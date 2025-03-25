using EPA.SyncJob.Constants;
using EPA.SyncJob.Helpers;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using EPA.SyncJob.Services.Interfaces;
using EPA.SyncJob.Workers;
using Microsoft.Extensions.Logging;

namespace EPA.SyncJob.Worker
{
    public class SyncJobLevelWorker : BaseWorker
    {
        private readonly IJobLevelService jobLevelService;
        private readonly ITokenService tokenService;
        private readonly ICompanyService companyService;

        public SyncJobLevelWorker(
            IJobLevelService jobLevelService,
            ITokenService tokenService,
            ICompanyService companyService,
            List<JobSetting> jobSettings,
            ILogger<SyncJobLevelWorker> logfile,
            Fanex.Logging.ILogger logger) : base(logfile, logger)
        {
            
            jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncJobLevel);
            this.jobLevelService = jobLevelService;
            this.companyService = companyService;
            this.tokenService = tokenService;
        }

        public override async Task DoWorkAsync()
        {
            var token = await tokenService.Request();
            if (!token.IsValid())
            {
                throw new InvalidOperationException("Invalid token");
            }
            var fromDate = await jobLevelService.GetLastModifiedDateAsync(token);
            var data = await jobLevelService.GetJobLevelAsync(fromDate);
            if (data.Any())
            {
                await jobLevelService.UpdateAsync(data, token);
            }
        }

    }
}
