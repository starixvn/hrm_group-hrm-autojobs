using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Job.Constants;
using HRM.Job.Criteria;
using HRM.Job.Criteria.AnnualLeave;
using HRM.Job.Extensions;
using HRM.Job.Models;
using HRM.Job.Models.AnnualLeave;
using HRM.Job.Models.MailDeliver;
using HRM.Job.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HRM.Job.Services;

public class AnnualLeaveBalanceService(
    IHostEnvironment hostEnvironment,
    ILogger<AnnualLeaveBalanceService> logfile,
    IBaseService baseService,
    IBaseMailService mailService,
    ILetStalkService letStalkService)
    : IAnnualLeaveBalanceService
{
    private const string RemindUseALTemplatePath = "EmailTemplates/RemindUseAL.txt";
    private const string StatisticALBalanceTemplatePath = "EmailTemplates/StatisticALBalance.txt";
    private const int MaxLocalAL = 30;
    private const int MaxExpatAL = 37;

    private readonly ILogger logfile = logfile;

    public async Task InsertForNextYearByYearlyAsync(AnnualLeaveResetConfig config)
    {
        var employees = await GetEmployeesDetail(config.CompanyId);

        employees = employees.Where(e => e.ConfirmedDate != null).ToList();

        List<AnnualLeaveUpdateModel> annualLeaveUpdateModels = [];

        foreach (var employee in employees)
        {
            var model = new AnnualLeaveUpdateModel
            {
                EmpId = employee.EmpId,
                Year = DateTimeOffset.Now.Year + 1,
                AnnualLeaves = employee.AnnualLeave
            };

            var yearsOfService = CalculateLengthOfService(employee.JoinedDate, employee.MilitaryFromDate, employee.MilitaryToDate);

            if (config.IsEnableAdditionalAnnualLeaveBalance)
            {
                var extraLeave = (int)Math.Floor(yearsOfService / employee.ExtraLeaveForYearsOfService);
                var maxAllowed = employee.IsLocal ? MaxLocalAL : MaxExpatAL;
                var total = model.AnnualLeaves + model.ExtraLeaves;

                if (total > maxAllowed)
                {
                    // reduce only ExtraLeaves to cap total
                    extraLeave = Math.Max(0, maxAllowed - model.AnnualLeaves);
                }
                model.AnnualLeaves += extraLeave;
            }

            annualLeaveUpdateModels.Add(model);

        }

        var command = new UpdateAnnualLeaveBalanceByYearlyCriteria
        {
            ListAnnualLeaveJson = JsonConvert.SerializeObject(annualLeaveUpdateModels)
        };
        await baseService.ExecuteNonQueryAsync(command);
    }

    private static double CalculateLengthOfService(DateTime joinedDate, DateTime? militaryFromDate, DateTime? militaryToDate)
    {
        var today = DateTimeOffset.Now;
        var nextTermDate = new DateTimeOffset(
            today.Year + 1,
            1,
            1,
            0, 0, 0,
            today.Offset);

        var totalMonths = (nextTermDate.Year - joinedDate.Year) * 12 + nextTermDate.Month - joinedDate.Month;
        if (militaryToDate.HasValue && militaryFromDate.HasValue)
        {
            int militaryMonths = ((militaryToDate.Value.Year - militaryFromDate.Value.Year) * 12)
                                + militaryToDate.Value.Month - militaryFromDate.Value.Month;
            totalMonths -= militaryMonths;
        }

        int lengthOfService = (int)Math.Floor(totalMonths / 12.0);
        return lengthOfService;
    }

    public async Task InsertForNewEmployeesByYearlyAsync(AnnualLeaveResetConfig config)
    {
        var employees = await GetEmployeesDetail(config.CompanyId);

        var today = DateTimeOffset.Now.Date;
        employees = employees
            .Where(e => e.ConfirmedDate.HasValue 
                    && e.ConfirmedDate.Value.Date >= today.AddDays(-7)
                    && e.ConfirmedDate.Value.Date <= today)
            .ToList();

        List<AnnualLeaveUpdateModel> annualLeaveUpdateModels = [];

        foreach (var employee in employees)
        {
            var model = new AnnualLeaveUpdateModel
            {
                EmpId = employee.EmpId,
                Year = DateTimeOffset.Now.Year,
                AnnualLeaves = CalculateMonthOfServicesForNewEmployee(employee)
            };

            annualLeaveUpdateModels.Add(model);
        }

        var command = new UpdateAnnualLeaveBalanceByYearlyCriteria
        {
            ListAnnualLeaveJson = JsonConvert.SerializeObject(annualLeaveUpdateModels)
        };
        await baseService.ExecuteNonQueryAsync(command);
    }

    private static int CalculateMonthOfServicesForNewEmployee(EmployeeServiceDetail employee)
    {
        DateTimeOffset endOfYearDate = new(
                employee.ConfirmedDate.Value.Year,
                12,
                1,
                0, 0, 0,
                TimeSpan.Zero
            );

        int monthDiff = ((endOfYearDate.Year - employee.JoinedDate.Year) * 12 + endOfYearDate.Month - employee.JoinedDate.Month);

        if (employee.JoinedDate.Day <= 15)
        {
            monthDiff += 1;
        }

        decimal amount = (employee.AnnualLeave / 12m) * monthDiff;

        amount = Math.Round(Math.Round(amount, 1), 0);

        return (int)amount;
    }

    public async Task<IEnumerable<AnnualLeaveResetConfig>> GetAnnualLeaveResetConfigs()
    {
        var command = new GetAnnualLeaveResetConfigsCriteria();

        var companies =  await baseService.GetDataAsync<AnnualLeaveResetConfig>(command);

        var excludeNexCompanies = companies.Where(c => !CommonConstants.NexCompanyIds.Contains(c.CompanyId));

        return excludeNexCompanies;
    }

    public async Task InsertForNewEmployeesByJoinedDateAsync(AnnualLeaveResetConfig config)
    {
        var employees = await GetEmployeesDetail(config.CompanyId);

        const bool isCalculatingNewEmployee = true;
        var TWConfigs = await GetTWAnnualLeaveConfigs();

        var today = DateTimeOffset.Now;

        employees = employees
            .Where(e => {
                 return e.JoinedDate.Year == today.Year;
             })
            .ToList();

        var annualLeaveUpdateModels 
            = CalculateExtraLeavesForEmployeesByJoinedDate(employees, TWConfigs, isCalculatingNewEmployee);

        var command = new UpdateAnnualLeaveBalanceByJoinedDateCriteria
        {
            ListAnnualLeaveJson = JsonConvert.SerializeObject(annualLeaveUpdateModels)
        };
        await baseService.ExecuteNonQueryAsync(command);
    }

    public async Task InsertForNextYearByJoinedDateAsync(AnnualLeaveResetConfig config)
    {
        var employees = await GetEmployeesDetail(config.CompanyId);

        var today = DateTimeOffset.Now;
        
        employees = employees
            .Where(e => {
                var joinDateThisYear = new DateTimeOffset(today.Year, e.JoinedDate.Month, e.JoinedDate.Day, 0, 0, 0, today.Offset);
                var threeMonthsBeforeJoinDate = joinDateThisYear.AddMonths(-3);
                return threeMonthsBeforeJoinDate <= today;
            })
            .ToList();

        const bool isCalculatingNewEmployee = false;
        var TWConfigs = await GetTWAnnualLeaveConfigs();
        var annualLeaveUpdateModels
            = CalculateExtraLeavesForEmployeesByJoinedDate(employees, TWConfigs, isCalculatingNewEmployee);

        var command = new UpdateAnnualLeaveBalanceByJoinedDateCriteria
        {
            ListAnnualLeaveJson = JsonConvert.SerializeObject(annualLeaveUpdateModels)
        };
        await baseService.ExecuteNonQueryAsync(command);
    }

    private static List<AnnualLeaveUpdateModel> CalculateExtraLeavesForEmployeesByJoinedDate(
        IEnumerable<EmployeeServiceDetail> employees,
        IEnumerable<TWAnnualLeaveConfigModel> TWConfigs,
        bool isCalculatingNewEmployee)
    {
        List<AnnualLeaveUpdateModel> annualLeaveUpdateModels = [];
        var termDate = DateTimeOffset.Now;
        if (!isCalculatingNewEmployee)
        {
            termDate = termDate.AddMonths(3);
        }

        var maxConfigYear = TWConfigs.Max(c => c.Year);
        foreach (var employee in employees)
        {
            var model = new AnnualLeaveUpdateModel
            {
                EmpId = employee.EmpId,
                Year = DateTimeOffset.Now.Year
            };

            var joinedDate = employee.JoinedDate;
            var yearsOfService = termDate.Year - joinedDate.Year;

            var anniversaryDate = new DateTimeOffset(termDate.Year, joinedDate.Month, joinedDate.Day, 0, 0, 0, termDate.Offset);
            if (termDate < anniversaryDate)
            {
                yearsOfService--;
            }

            TWAnnualLeaveConfigModel config = yearsOfService > maxConfigYear
                ? TWConfigs.FirstOrDefault(c => c.Year == maxConfigYear)
                : TWConfigs.FirstOrDefault(c => c.Year == yearsOfService);

            model.AnnualLeaves = config.LaborBalance;
            model.ExtraLeaves = config.CompanyLeave;
            const int maxAllowed = MaxLocalAL;
            var total = model.AnnualLeaves + model.ExtraLeaves;

            if (total > maxAllowed)
            {
                // reduce only ExtraLeaves to cap total
                model.ExtraLeaves = Math.Max(0, maxAllowed - model.AnnualLeaves);
            }

            model.Year = termDate.Year;
            annualLeaveUpdateModels.Add(model);
        }

        return annualLeaveUpdateModels;
    } 

    private async Task<IEnumerable<EmployeeServiceDetail>> GetEmployeesDetail(short companyId)
    {
        var command = new GetEmployeesServiceDetailCriteria
        {
            CompanyId = companyId
        };
        var employees = await baseService.GetDataAsync<EmployeeServiceDetail>(command);
        return employees;
    }

    private async Task<IEnumerable<TWAnnualLeaveConfigModel>> GetTWAnnualLeaveConfigs()
    {
        var command = new GetTWLeaveBalanceConfigsCriteria();
        return await baseService.GetDataAsync<TWAnnualLeaveConfigModel>(command);
    }

    public async Task RemindUsedALAsync(short companyId)
    {
        var year = DateTimeOffset.Now.Year;
        var criteria = new GetAnnualLeaveBalanceCriteria
        {
            Year = year,
            CompanyId = companyId
        };
        var (employees, supervisors, statistics)
            = await baseService.FetchMultipleAsync<AnnualLeaveBalance, EmployeeSupervisor, SectionStatisticAnnualLeave>(criteria);
        var bodyTemplate = ReadTextFile(Path.Combine(hostEnvironment.ContentRootPath, RemindUseALTemplatePath));
        var validEmployees = new List<AnnualLeaveBalance>();
        var mailConfig = await mailService.GetMailConfigAsync(companyId);

        if (mailConfig is not { Enable: true })
        {
            return;
        }

        var mailSetting = await mailService.GetMailSettingByIdAsync(mailConfig.AppMailId);

        foreach (var employee in employees)
        {
            var percentAnnualLeaveRemaining = employee.LeaveBalance / employee.AnnualLeaveCurrentYear * 100;
            var isSentMail = false;
            var percentInMail = string.Empty;

            CheckSendMail(ref isSentMail, employee.IsLocal, ref percentInMail, percentAnnualLeaveRemaining);

            validEmployees.Add(employee);

            var body = string.Format(
                bodyTemplate,
                employee.Nickname,
                percentInMail,
                year + 1,
                $"{employee.LeaveBalance}/{employee.AnnualLeaveCurrentYear}");

            if (!string.IsNullOrWhiteSpace(employee.Email) && isSentMail)
            {
                var mailItem = new MailRecord
                {
                    MailTo = employee.Email,
                    MailCc = string.Empty,
                    Subject = "Annual Leave Balance Reminder",
                    Body = body,
                    MailSetting = mailSetting
                };

                await Task.Run(async () => await SendEmailAsync(mailItem));
            }

            await Task.Run(async () => await SendLetStalkNotificationAsync(employee.LetStalk, "Annual Leave Balance Reminder", body));
        }

        await SendMailStatisticAnnualLeave(validEmployees, supervisors, statistics.ToList());
    }

    private Task SendMailStatisticAnnualLeave(
        List<AnnualLeaveBalance> validEmployees,
        IEnumerable<EmployeeSupervisor> supervisors,
        List<SectionStatisticAnnualLeave> statistics)
    {
        foreach (var item in statistics)
        {
            var totalEmployees = validEmployees.Count(x => x.DepartmentId == item.DepartmentId);
            var percentage = Math.Round((double)totalEmployees / item.TotalEmployee * 100);

            item.AnnualLeaveBalance = totalEmployees;
            item.Percentage = percentage;
        }

        var percentByMonth = GetPercentByMonth();

        var groupSupervisors = supervisors
            .GroupBy(g => new { g.Nickname, g.OwnerId, g.DepartmentId, g.Email })
            .GroupBy(x => x.Key.OwnerId).ToList();

        const string tableALTemplate = "<tr><td style=\"border: 1px solid;\">{0}</td>"
                                       + "<td style=\"text-align: center; border: 1px solid;\">{1}</td>"
                                       + "<td style=\"text-align: center; border: 1px solid;\">{2}</td>"
                                       + "<td style=\"text-align: center; border: 1px solid;\">{3}%</td></tr>";

        foreach (var groupSupervisor in groupSupervisors)
        {
            var tableStatistic = new StringBuilder();
            var nickname = groupSupervisor.First().Key.Nickname;
            var email = groupSupervisor.First().Key.Email;

            var sections = groupSupervisor.Select(x => x.Key).ToList();
            foreach (var item in sections)
            {
                var statisticSection = statistics.Find(x => x.DepartmentId == item.DepartmentId);
                if (statisticSection != null)
                {
                    tableStatistic.AppendFormat(
                        tableALTemplate,
                        statisticSection.DepartmentName,
                        statisticSection.TotalEmployee,
                        statisticSection.AnnualLeaveBalance,
                        statisticSection.Percentage
                    );
                }
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                var bodyTemplate = ReadTextFile(Path.Combine(hostEnvironment.ContentRootPath, StatisticALBalanceTemplatePath));
                var body = string.Format(
                    bodyTemplate,
                    nickname,
                    percentByMonth,
                    tableStatistic,
                    DateTimeOffset.Now.Year + 1);

                var mailItem = new MailRecord
                {
                    MailTo = email,
                    MailCc = string.Empty,
                    Subject = "Department Annual Leave Balance Reminder",
                    Body = body
                };

                Task.Run(async () => await SendEmailAsync(mailItem));
            }
        }

        return Task.CompletedTask;
    }

    private async Task SendEmailAsync(MailRecord mailRecord)
    {
        try
        {
            await mailService.SendEmailAsync(mailRecord);
        }
        catch (Exception ex)
        {
            logfile.LogError(ex, ex.Message);
        }
    }

    private async Task SendLetStalkNotificationAsync(string letStalkId, string subject, string message)
    {
        try
        {
            await letStalkService.SendNotificationAsync(letStalkId, subject, message);
        }
        catch (Exception ex)
        {
            logfile.LogError(ex, ex.Message);
        }
    }

    private static string ReadTextFile(string filePath)
    {
        using var sr = new StreamReader(filePath);
        return sr.ReadToEnd();
    }

    private static string GetPercentByMonth()
    {
        var percent = string.Empty;
        const byte March = 3;
        const byte June = 6;
        const byte September = 9;
        const byte December = 12;

        percent = DateTimeOffset.Now.Month switch
        {
            March => "> 75%",
            June => "> 50%",
            September => "> 25%",
            December => "> 0%",
            _ => percent
        };
        return percent;
    }

    private static void CheckSendMail(ref bool isSentMail, bool isLocal, ref string percentInMail, float percentAnnualLeaveRemaining)
    {
        const byte March = 3;
        const byte June = 6;
        const byte September = 9;
        const byte December = 12;
        const byte TwentyFive = 25;
        const byte Fifty = 50;
        const byte SeventyFive = 75;
        switch (DateTimeOffset.Now.DateTime.ToClientTimeZone(8).Month)
        {
            case March:
                if (!isLocal && percentAnnualLeaveRemaining > SeventyFive)
                {
                    isSentMail = true;
                    percentInMail = "over 75% ";
                }
                break;

            case June:
                if (percentAnnualLeaveRemaining > Fifty)
                {
                    isSentMail = true;
                    percentInMail = "over 50% ";
                }
                break;

            case September:
                if (!isLocal && percentAnnualLeaveRemaining > TwentyFive)
                {
                    isSentMail = true;
                    percentInMail = "over 25% ";
                }
                break;

            case December:
                if (percentAnnualLeaveRemaining > 0)
                {
                    isSentMail = true;
                }
                break;

            default:
                percentInMail = string.Empty;
                break;
        }
    }
}