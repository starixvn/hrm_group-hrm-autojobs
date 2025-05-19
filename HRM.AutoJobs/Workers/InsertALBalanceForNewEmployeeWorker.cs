namespace HRM.AutoJobs.Workers;

using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using HRM.Job.Enum;
using HRM.Job.Models.AnnualLeave;
using Job.Constants;
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