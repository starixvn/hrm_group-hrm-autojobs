using HRM.Job.Constants;
using HRM.Job.Helpers;
using HRM.Job.Models;
using HRM.Job.Services.Interfaces;

namespace HRM.AutoJobs.Workers;

using System.Collections.Generic;
using System.Threading.Tasks;

public class SyncWorkingShiftWorker : BaseWorker
{
    private readonly ISyncWorkingShiftService syncWorkingShiftService;

    public SyncWorkingShiftWorker(
        ISyncWorkingShiftService syncWorkingShiftService,
        List<JobSetting> jobSettings,
        ILogger<SyncWorkingShiftWorker> logfile,
        Fanex.Logging.ILogger logger) : base(logfile, logger)
    {
        this.syncWorkingShiftService = syncWorkingShiftService;
        jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncWorkingShiftFromOWP);
    }

    public override async Task DoWorkAsync()
    {
        await syncWorkingShiftService.SyncWorkingShiftAsync();
    }
}