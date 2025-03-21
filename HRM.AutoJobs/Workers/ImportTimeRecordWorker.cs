using HRM.Job.Constants;
using HRM.Job.Helpers;
using HRM.Job.Models;
using HRM.Job.Services.Interfaces;

namespace HRM.AutoJobs.Workers;

using System.Collections.Generic;
using System.Threading.Tasks;

public class ImportTimeRecordWorker : BaseWorker
{
    private readonly ITimeRecordService timeRecordService;

    public ImportTimeRecordWorker(
        ITimeRecordService timeRecordService,
        List<JobSetting> jobSettings,
        ILogger<ImportTimeRecordWorker> logfile,
        Fanex.Logging.ILogger logger) : base(logfile, logger)
    {
        this.timeRecordService = timeRecordService;
        jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.ImportTimeRecord);
    }

    public override async Task DoWorkAsync()
    {
        await timeRecordService.ImportTimeRecordAsync();
    }
}