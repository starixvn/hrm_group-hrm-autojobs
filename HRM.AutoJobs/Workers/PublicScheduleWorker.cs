using HRM.Job.Constants;
using HRM.Job.Helpers;
using HRM.Job.Models;
using HRM.Job.Services.Interfaces;

namespace HRM.AutoJobs.Workers;

using System.Collections.Generic;
using System.Threading.Tasks;


public class PublicScheduleWorker : BaseWorker
{
    private readonly IScheduleArrangeService scheduleArrangeService;

    public PublicScheduleWorker(
        IScheduleArrangeService scheduleArrangeService,
        List<JobSetting> jobSettings,
        ILogger<PublicScheduleWorker> logfile,
        Fanex.Logging.ILogger logger) : base(logfile, logger)
    {
        this.scheduleArrangeService = scheduleArrangeService;
        jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.PublicSchedule);
    }

    public override async Task DoWorkAsync()
    {
        await scheduleArrangeService.PublicScheduleAsync(jobSetting.CompanyId);
    }
}