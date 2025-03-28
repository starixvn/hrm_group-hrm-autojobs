namespace HRM.AutoJobs.Workers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Job.Constants;
using Job.Helpers;
using Job.Models;
using Job.Services.Interfaces;
using Microsoft.Extensions.Logging;

public class RemindUsedALWorker : BaseWorker
{
    private readonly IAnnualLeaveBalanceService annualLeaveBalanceService;

    public RemindUsedALWorker(
        IAnnualLeaveBalanceService annualLeaveBalanceService,
        List<JobSetting> jobSettings,
        ILogger<RemindUsedALWorker> logfile,
        Fanex.Logging.ILogger logger) : base(logfile, logger)
    {
        this.annualLeaveBalanceService = annualLeaveBalanceService;
        jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.RemindUsedAL);
    }

    public override async Task DoWorkAsync()
    {
        var months = new[] { 2, 5, 8, 11 };
        var today = DateTimeOffset.UtcNow;
        var lastDay = DateTime.DaysInMonth(today.Year, today.Month);

        if (today.Day == lastDay && months.Contains(today.Month))
        {
            foreach (var companyId in jobSetting.CompanyIds)
            {
                await annualLeaveBalanceService.RemindUsedALAsync(companyId);
            }
        }
    }
}