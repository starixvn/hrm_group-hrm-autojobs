using HRM.Job.Constants;
using HRM.Job.Helpers;
using HRM.Job.Models;
using HRM.Job.Services.Interfaces;

namespace HRM.AutoJobs.Workers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        if (DateTimeOffset.UtcNow.DayOfWeek == DayOfWeek.Friday)
        {
            await scheduleArrangeService.InsertDefaultShiftAsync();
        }
    }
}