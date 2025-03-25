namespace HRM.AutoJobs.Workers;

using System.Collections.Generic;
using System.Threading.Tasks;
using Job.Constants;
using Job.Helpers;
using Job.Models;
using Job.Services.Interfaces;
using Microsoft.Extensions.Logging;

public class SyncTimeRecordWorker : BaseWorker
{
    private readonly ITimeRecordService timeRecordService;

    public SyncTimeRecordWorker(
        ITimeRecordService timeRecordService,
        List<JobSetting> jobSettings,
        ILogger<SyncTimeRecordWorker> logfile,
        Fanex.Logging.ILogger logger) : base(logfile, logger)
    {
        this.timeRecordService = timeRecordService;
        jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncTimeRecordFromAPI);
    }

    public override async Task DoWorkAsync()
    {
        await timeRecordService.ImportTimeRecordAsync();
    }
}