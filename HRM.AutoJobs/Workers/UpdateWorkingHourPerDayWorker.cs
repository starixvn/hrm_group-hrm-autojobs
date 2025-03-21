using HRM.Job.Constants;
using HRM.Job.Helpers;
using HRM.Job.Models;
using HRM.Job.Services.Interfaces;

namespace HRM.AutoJobs.Workers;

using System.Collections.Generic;
using System.Threading.Tasks;

public class UpdateWorkingHourPerDayWorker : BaseWorker
{
    private readonly IPostnatalService postnatalService;

    public UpdateWorkingHourPerDayWorker(
        IPostnatalService postnatalService,
        List<JobSetting> jobSettings,
        ILogger<UpdateWorkingHourPerDayWorker> logfile,
        Fanex.Logging.ILogger logger) : base(logfile, logger)
    {
        this.postnatalService = postnatalService;
        jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.UpdateWorkingHourPerDay);
    }

    public override async Task DoWorkAsync()
    {
        await postnatalService.UpdateWorkingHourPerDayAsync();
    }
}