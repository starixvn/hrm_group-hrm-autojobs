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
        private readonly List<ServiceAPI> serviceApis;
        private readonly ILogger<SyncGroupDisciplinePermissionWorker> logfile;

        public SyncGroupDisciplinePermissionWorker(
            IDisciplinePermissionService disciplineService,
            ITokenService tokenService,
            ICompanyService companyService,
            List<JobSetting> jobSettings,
            List<ServiceAPI> serviceApis,
            ILogger<SyncGroupDisciplinePermissionWorker> logfile,
            Fanex.Logging.ILogger logger) : base(logfile, logger)
        {
            
            jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncGroupDiscipline);
            this.disciplineService = disciplineService;
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
                    logfile.LogInformation($"Successful SyncGroupDiscipline service API: {serviceApi.Name}");
                }
                catch (Exception ex)
                {
                    logfile.LogError($"Error SyncGroupDiscipline service API: {serviceApi.Name} - {ex}");
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
            var fromDate = await disciplineService.GetLastModifiedDateForGroupAsync(token, serviceApi);
            var data = await disciplineService.GetGroupsSync(fromDate, companyData.CompanyId);
            if (data.Any())
            {
                await disciplineService.UpdateGroupsAsync(data, token, serviceApi);
            }
        }

    }
}
