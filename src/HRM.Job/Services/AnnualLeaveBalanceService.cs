using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Job.Criteria;
using HRM.Job.Extensions;
using HRM.Job.Models;
using HRM.Job.Models.MailDeliver;
using HRM.Job.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

    private readonly ILogger logfile = logfile;

    public async Task InsertForNextYearAsync(short companyId)
    {
        var command = new UpdateAnnualLeaveBalanceCommand
        {
            Year = DateTimeOffset.Now.Year + 1,
            CompanyId = companyId
        };
        await baseService.ExecuteNonQueryAsync(command);
    }

    public async Task InsertForNewEmployeeAsync(short companyId)
    {
        var command = new InsertAnnualLeaveBalanceForNewEmployeeCommand
        {
            Year = DateTimeOffset.Now.Year,
            CompanyId = companyId
        };
        await baseService.ExecuteNonQueryAsync(command);
    }

    public async Task RemindUsedALAsync(short companyId)
    {
        var year = DateTimeOffset.Now.Year;
        var criteria = new GetAnnualLeaveBalanceCriteria
        {
            Year = year,
            CompanyId = companyId
        };
        var (employees, supervisors, statistics) = await baseService.FetchMultipleAsync<AnnualLeaveBalance, EmployeeSupervisor, SectionStatisticAnnualLeave>(criteria);
        var bodyTemplate = ReadTextFile(Path.Combine(hostEnvironment.ContentRootPath, RemindUseALTemplatePath));
        var validEmployees = new List<AnnualLeaveBalance>();
        var mailConfig = await mailService.GetMailConfigAsync(companyId);
        var mailSetting = await mailService.GetMailSettingByIdAsync(mailConfig?.AppMailId ?? 0);

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

#pragma warning disable S1541 // Methods and properties should not be too complex

    private static void CheckSendMail(ref bool isSentMail, bool isLocal, ref string percentInMail, float percentAnnualLeaveRemaining)
#pragma warning restore S1541 // Methods and properties should not be too complex
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