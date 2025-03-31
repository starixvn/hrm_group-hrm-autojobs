using EPA.SyncJob.Constants;
using EPA.SyncJob.Helpers;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using EPA.SyncJob.Services.Interfaces;
using EPA.SyncJob.Workers;

namespace EPA.SyncJob.Worker
{
    public class SyncJobTitleWorker : BaseWorker
    {
        private readonly IJobTitleService jobTitleService;
        private readonly ITokenService tokenService;
        private readonly ICompanyService companyService;
        private readonly List<ServiceAPI> serviceApis;
        private readonly ILogger<SyncJobTitleWorker> logfile;
        private readonly Fanex.Logging.ILogger logger;

        public SyncJobTitleWorker(
            IJobTitleService jobTitleService,
            ITokenService tokenService,
            ICompanyService companyService,
            List<JobSetting> jobSettings,
            List<ServiceAPI> serviceApis,
            ILogger<SyncJobTitleWorker> logfile,
            Fanex.Logging.ILogger logger) : base(logfile, logger)
        {
            
            jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncJobTitle);
            this.jobTitleService = jobTitleService;
            this.companyService = companyService;
            this.tokenService = tokenService;
            this.serviceApis = serviceApis;
            this.logfile = logfile;
            this.logger = logger;
        }

        public override async Task DoWorkAsync()
        {
            var serviceApiList = serviceApis.ToList();
            var tasks = serviceApiList.Select(async serviceApi =>
            {
                try
                {
                    await UpdateAsync(serviceApi);
                    logfile.LogInformation($"Successful  SyncJobTitle service API: {serviceApi.Name}");
                }
                catch (Exception ex)
                {
                    await logger.ErrorAsync($"Error SyncJobTitle  service API: {serviceApi.Name} - {ex}");
                    logfile.LogError($"Error SyncJobTitle  service API: {serviceApi.Name} - {ex}");
                }
            });

            await Task.WhenAll(tasks);
        }

        private async Task UpdateAsync(ServiceAPI serviceApi)
        {
            var token = await tokenService.Request(serviceApi);
            if (!token.IsValid())
            {
                throw new InvalidOperationException("Invalid token");
            }
            var data = await jobTitleService.GetJobTitles(serviceApi.CompanyId);
            if (data.Any())
            {
                await jobTitleService.UpdateAsync(data, token, serviceApi);
            }
        }

    }
}
