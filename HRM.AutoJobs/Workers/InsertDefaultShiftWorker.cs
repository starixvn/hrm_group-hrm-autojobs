namespace HRM.AutoJobs.Workers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Job.Constants;
using Job.Helpers;
using Job.Models;
using Job.Services.Interfaces;
using Microsoft.Extensions.Logging;

public class InsertDefaultShiftWorker : BaseWorker
{
    private readonly IScheduleArrangeService scheduleArrangeService;

    public InsertDefaultShiftWorker(
        IScheduleArrangeService scheduleArrangeService,
        List<JobSetting> jobSettings,
        ILogger<InsertDefaultShiftWorker> logfile,
        Fanex.Logging.ILogger logger) : base(logfile, logger)
    {
        this.scheduleArrangeService = scheduleArrangeService;
        jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.InsertDefaultShift);
    }

    public override async Task DoWorkAsync()
    {
        //if (DateTimeOffset.UtcNow.DayOfWeek == DayOfWeek.Friday)
        {
            foreach (var companyId in jobSetting.CompanyIds)
            {
                await scheduleArrangeService.InsertDefaultShiftAsync(companyId);
            }
        }
    }
}