using HRM.Job.Constants;
using HRM.Job.Helpers;
using HRM.Job.Models;
using HRM.Job.Services.Interfaces;

namespace HRM.AutoJobs.Workers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        const byte September = 9;
        const byte December = 12;
        var today = DateTimeOffset.UtcNow;
        if (today is { Day: 1, Month: September }
            || (today.Day == DateTime.DaysInMonth(today.Year, today.Month) && today.Month == December))
        {
            await annualLeaveBalanceService.InsertForNextYearAsync();
        }
    }
}