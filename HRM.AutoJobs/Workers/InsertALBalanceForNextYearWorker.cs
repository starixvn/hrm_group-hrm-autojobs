namespace HRM.AutoJobs.Workers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Job.Constants;
using Job.Helpers;
using Job.Models;
using Job.Services.Interfaces;
using System.Linq;

public class InsertALBalanceForNextYearWorker : BaseWorker
{
    private readonly IAnnualLeaveBalanceService annualLeaveBalanceService;

    public InsertALBalanceForNextYearWorker(
        IAnnualLeaveBalanceService annualLeaveBalanceService,
        List<JobSetting> jobSettings,
        ILogger<InsertALBalanceForNextYearWorker> logfile,
        Fanex.Logging.ILogger logger) : base(logfile, logger)
    {
        this.annualLeaveBalanceService = annualLeaveBalanceService;
        jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.InsertALBalanceForNextYear);
    }

    public override async Task DoWorkAsync()
    {
        var today = DateTimeOffset.UtcNow;
        var lastDay = DateTime.DaysInMonth(today.Year, today.Month);
        var lastDayMonths = new[] { 8, 11 };

        if (today.Day == lastDay && lastDayMonths.Contains(today.Month))
        {
            foreach (var companyId in jobSetting.CompanyIds)
            {
                await annualLeaveBalanceService.InsertForNextYearAsync(companyId);
            }
        }
    }
}