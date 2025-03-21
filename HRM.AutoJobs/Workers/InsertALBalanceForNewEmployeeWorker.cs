using HRM.Job.Constants;
using HRM.Job.Helpers;
using HRM.Job.Models;
using HRM.Job.Services.Interfaces;

namespace HRM.AutoJobs.Workers;

using System.Collections.Generic;
using System.Threading.Tasks;

public class InsertALBalanceForNewEmployeeWorker : BaseWorker
{
    private readonly IAnnualLeaveBalanceService annualLeaveBalanceService;

    public InsertALBalanceForNewEmployeeWorker(
        IAnnualLeaveBalanceService annualLeaveBalanceService,
        List<JobSetting> jobSettings,
        ILogger<InsertALBalanceForNewEmployeeWorker> logfile,
        Fanex.Logging.ILogger logger) : base(logfile, logger)
    {
        this.annualLeaveBalanceService = annualLeaveBalanceService;
        jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.InsertALBalanceForNewEmployee);
    }

    public override async Task DoWorkAsync()
    {
        await annualLeaveBalanceService.InsertForNewEmployeeAsync();
    }
}