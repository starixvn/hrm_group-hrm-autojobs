using HRM.Job.Constants;
using HRM.Job.Helpers;
using HRM.Job.Models;
using HRM.Job.Services.Interfaces;

namespace HRM.AutoJobs.Workers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class InsertWeeklyDefaultShiftWorker : BaseWorker
{
    private readonly IScheduleArrangeService scheduleArrangeService;

    public InsertWeeklyDefaultShiftWorker(
        IScheduleArrangeService scheduleArrangeService,
        List<JobSetting> jobSettings,
        ILogger<InsertWeeklyDefaultShiftWorker> logfile,
        Fanex.Logging.ILogger logger) : base(logfile, logger)
    {
        this.scheduleArrangeService = scheduleArrangeService;
        jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.InsertWeeklyDefaultShift);
    }

    public override async Task DoWorkAsync()
    {
        if (DateTimeOffset.UtcNow.DayOfWeek == DayOfWeek.Friday)
        {
            await scheduleArrangeService.InsertWeeklyDefaultShiftAsync();
        }
    }
}