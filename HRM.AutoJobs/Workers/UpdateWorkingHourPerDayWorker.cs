namespace HRM.AutoJobs.Workers;

using System.Collections.Generic;
using System.Threading.Tasks;
using Job.Constants;
using Job.Helpers;
using Job.Models;
using Job.Services.Interfaces;
using Microsoft.Extensions.Logging;

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