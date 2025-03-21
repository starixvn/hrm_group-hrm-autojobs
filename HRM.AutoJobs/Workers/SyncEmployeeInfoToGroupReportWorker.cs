using HRM.Job.Constants;
using HRM.Job.Helpers;
using HRM.Job.Models;
using HRM.Job.Services.Interfaces;

namespace HRM.AutoJobs.Workers;

using System.Collections.Generic;
using System.Threading.Tasks;

public class SyncEmployeeInfoToGroupReportWorker : BaseWorker
{
    private readonly IGroupReportService groupReportService;

    public SyncEmployeeInfoToGroupReportWorker(
        IGroupReportService groupReportService,
        List<JobSetting> jobSettings,
        ILogger<SyncWorkingShiftWorker> logfile,
        Fanex.Logging.ILogger logger) : base(logfile, logger)
    {
        this.groupReportService = groupReportService;
        jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncEmployeeInfoToGroupReport);
    }

    public override async Task DoWorkAsync()
    {
        await groupReportService.SyncEmployeeInfoAsync();
    }
}