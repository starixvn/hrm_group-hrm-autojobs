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

        public SyncDisciplineSummaryWorker(
            IDisciplineSummaryService disciplineSummaryService,
            ITokenService tokenService,
            ICompanyService companyService,
            List<JobSetting> jobSettings,
            ILogger<SyncDisciplineSummaryWorker> logfile,
            Fanex.Logging.ILogger logger) : base(logfile, logger)
        {
            
            jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncDisciplineSummary);
            this.disciplineSummaryService = disciplineSummaryService;
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
            var fromDate = await disciplineSummaryService.GetLastModifiedDateAsync(token);
            var data= await disciplineSummaryService.GetDisciplineSummaryAsync(fromDate, companyData.CompanyId);
            if (data.Any())
            {
                await disciplineSummaryService.UpdateAsync(data, token);
            }
        }

    }
}
