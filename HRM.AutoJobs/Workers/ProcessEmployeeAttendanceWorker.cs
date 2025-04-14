using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Job.Constants;
using HRM.Job.Helpers;
using HRM.Job.Models;
using HRM.Job.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace HRM.AutoJobs.Workers;

public class ProcessEmployeeAttendanceWorker : BaseWorker
{
    private readonly ITimeRecordService timeRecordService;

    public ProcessEmployeeAttendanceWorker(
        ITimeRecordService timeRecordService,
        List<JobSetting> jobSettings,
        ILogger<ProcessEmployeeAttendanceWorker> logfile,
        Fanex.Logging.ILogger logger) : base(logfile, logger)
    {
        this.timeRecordService = timeRecordService;
        jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.ProcessEmployeeAttendance);
    }

    public override async Task DoWorkAsync()
    {
        await timeRecordService.ProcessEmployeeAttendanceAsync(jobSetting.CompanyIds);
    }
}