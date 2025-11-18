namespace HRM.AutoJobs.Workers;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Job.Constants;
using Job.Helpers;
using Job.Models;
using Job.Enum;
using Job.Services.Interfaces;

public class InsertALBalanceForNextYearWorker : BaseWorker
{
    private readonly IAnnualLeaveBalanceService annualLeaveBalanceService;

    public InsertALBalanceForNextYearWorker(
        IAnnualLeaveBalanceService annualLeaveBalanceService,
        List<JobSetting> jobSettings,
        ILogger<InsertALBalanceForNextYearWorker> logfile,
        Fanex.Logging.ILogger logger) : base(logfile, logger)
    {
        this.annualLeaveBalanceService = annualLeaveBalanceService;
        jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.InsertALBalanceForNextYear);
    }

    public override async Task DoWorkAsync()
    {
        var configs = await annualLeaveBalanceService.GetAnnualLeaveResetConfigs();

        foreach (var config in configs)
        {
            switch (config.Timeline)
            {
                case AnnualLeaveTimeline.JoinedDate:
                    await annualLeaveBalanceService.InsertForNextYearByJoinedDateAsync(config);
                    break;
                case AnnualLeaveTimeline.Yearly:
                default:
                    await annualLeaveBalanceService.InsertForNextYearByYearlyAsync(config);
                    break;
            }
        }
    }
}