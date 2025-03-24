using EPA.SyncJob.Constants;
using EPA.SyncJob.Helpers;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using EPA.SyncJob.Services.Interfaces;
using EPA.SyncJob.Workers;
using Microsoft.Extensions.Logging;

namespace EPA.SyncJob.Worker
{
    public class SyncJobTitleWorker : BaseWorker
    {
        private readonly IJobTitleService jobTitleService;
        private readonly ITokenService tokenService;
        private readonly ICompanyService companyService;

        public SyncJobTitleWorker(
            IJobTitleService jobTitleService,
            ITokenService tokenService,
            ICompanyService companyService,
            List<JobSetting> jobSettings,
            ILogger<SyncJobTitleWorker> logfile,
            Fanex.Logging.ILogger logger) : base(logfile, logger)
        {
            
            jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncJobTitle);
            this.jobTitleService = jobTitleService;
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
            var companyData = await companyService.GetCompanySiteByDomainAsync();
            if (companyData == null)
            {
                throw new InvalidOperationException("Invalid company url");
            }
            var data= await jobTitleService.GetJobTitles(companyData.CompanyId);
            if (data.Any())
            {
                await jobTitleService.UpdateAsync(data, token);
            }
        }

    }
}
