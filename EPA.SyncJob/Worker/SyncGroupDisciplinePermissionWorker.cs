using EPA.SyncJob.Constants;
using EPA.SyncJob.Helpers;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using EPA.SyncJob.Services.Interfaces;
using EPA.SyncJob.Workers;
using Microsoft.Extensions.Logging;

namespace EPA.SyncJob.Worker
{
    public class SyncGroupDisciplinePermissionWorker : BaseWorker
    {
        private readonly IDisciplinePermissionService disciplineService;
        private readonly ITokenService tokenService;
        private readonly ICompanyService companyService;

        public SyncGroupDisciplinePermissionWorker(
            IDisciplinePermissionService disciplineService,
            ITokenService tokenService,
            ICompanyService companyService,
            List<JobSetting> jobSettings,
            ILogger<SyncGroupDisciplinePermissionWorker> logfile,
            Fanex.Logging.ILogger logger) : base(logfile, logger)
        {
            
            jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncGroupDiscipline);
            this.disciplineService = disciplineService;
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
            var fromDate = await disciplineService.GetLastModifiedDateForGroupAsync(token);
            var data= await disciplineService.GetGroupsSync(fromDate, companyData.CompanyId);
            if (data.Any())
            {
                await disciplineService.UpdateGroupsAsync(data, token);
            }
        }

    }
}
