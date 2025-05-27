namespace HRM.AutoJobs.Workers;

using System.Collections.Generic;
using System.Threading.Tasks;
using Job.Constants;
using Job.Enum;
using Job.Helpers;
using Job.Models;
using Job.Services.Interfaces;
using Microsoft.Extensions.Logging;

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
        var configs = await annualLeaveBalanceService.GetAnnualLeaveResetConfigs();

        foreach (var config in configs)
        {
            switch (config.Timeline)
            {
                case TimeLineEnum.JoinedDate:
                    await annualLeaveBalanceService.InsertForNewEmployeesByJoinedDateAsync(config);
                    break;

                case TimeLineEnum.Yearly:
                default:
                    await annualLeaveBalanceService.InsertForNewEmployeeAsync(config.CompanyId);
                    break;
            }
        };
    }
}