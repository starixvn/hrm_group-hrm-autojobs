using EPA.SyncJob.Constants;
using EPA.SyncJob.Helpers;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using EPA.SyncJob.Services.Interfaces;
using EPA.SyncJob.Workers;
using Microsoft.Extensions.Logging;

namespace EPA.SyncJob.Worker
{
    public class SyncDisciplineSummaryWorker : BaseWorker
    {
        private readonly IDisciplineSummaryService disciplineSummaryService;
        private readonly ITokenService tokenService;
        private readonly ICompanyService companyService;
        private readonly List<ServiceAPI> serviceApis;
        private readonly ILogger<SyncDisciplineSummaryWorker> logfile;

        public SyncDisciplineSummaryWorker(
            IDisciplineSummaryService disciplineSummaryService,
            ITokenService tokenService,
            ICompanyService companyService,
            List<JobSetting> jobSettings,
            List<ServiceAPI> serviceApis,
            ILogger<SyncDisciplineSummaryWorker> logfile,
            Fanex.Logging.ILogger logger) : base(logfile, logger)
        {
            
            jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncDisciplineSummary);
            this.disciplineSummaryService = disciplineSummaryService;
            this.companyService = companyService;
            this.tokenService = tokenService;
            this.serviceApis = serviceApis;
            this.logfile = logfile;
        }

        public override async Task DoWorkAsync()
        {
            var serviceApiList = serviceApis.ToList();
            var tasks = serviceApiList.Select(async serviceApi =>
            {
                try
                {
                    await UpdateAsync(serviceApi);
                    logfile.LogInformation($"Successful SyncDisciplineSummary service API: {serviceApi.Name}");
                }
                catch (Exception ex)
                {
                    logfile.LogError($"Error SyncDiscipline service API: {serviceApi.Name} - {ex}");
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
            var companyData = await companyService.GetCompanySiteByDomainAsync(serviceApi.Domain);
            if (companyData == null)
            {
                throw new InvalidOperationException("Invalid company url");
            }
           var fromDate = await disciplineSummaryService.GetLastModifiedDateAsync(token, serviceApi);
            var data = await disciplineSummaryService.GetDisciplineSummaryAsync(fromDate, companyData.CompanyId);
            if (data.Any())
            {
                await disciplineSummaryService.UpdateAsync(data, token, serviceApi);
            }
        }

    }
}
