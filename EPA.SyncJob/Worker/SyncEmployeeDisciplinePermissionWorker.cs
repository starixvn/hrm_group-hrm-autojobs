using EPA.SyncJob.Constants;
using EPA.SyncJob.Helpers;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using EPA.SyncJob.Services.Interfaces;
using EPA.SyncJob.Workers;

namespace EPA.SyncJob.Worker
{
    public class SyncEmployeeDisciplinePermissionWorker : BaseWorker
    {
        private readonly IDisciplinePermissionService disciplineService;
        private readonly ITokenService tokenService;
        private readonly ICompanyService companyService;
        private readonly List<ServiceAPI> serviceApis;
        private readonly ILogger<SyncEmployeeDisciplinePermissionWorker> logfile;
        private readonly Fanex.Logging.ILogger logger;

        public SyncEmployeeDisciplinePermissionWorker(
            IDisciplinePermissionService disciplineService,
            ITokenService tokenService,
            ICompanyService companyService,
            List<JobSetting> jobSettings,
            List<ServiceAPI> serviceApis,
            ILogger<SyncEmployeeDisciplinePermissionWorker> logfile,
            Fanex.Logging.ILogger logger) : base(logfile, logger)
        {
            
            jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncEmployeeDiscipline);
            this.disciplineService = disciplineService;
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
                    logfile.LogInformation($"Successful SyncEmployeeDiscipline service API: {serviceApi.Name}");
                }
                catch (Exception ex)
                {
                    await logger.ErrorAsync($"Error SyncEmployeeDiscipline service API: {serviceApi.Name} - {ex}");
                    logfile.LogError($"Error SyncEmployeeDiscipline service API: {serviceApi.Name} - {ex}");
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
            var fromDate = await disciplineService.GetLastModifiedDateForEmployeeAsync(token, serviceApi);
            var data = await disciplineService.GetEmployeesAsync(fromDate, serviceApi.CompanyId);
            if (data.Any())
            {
                await disciplineService.UpdateEmployeesAsync(data, token, serviceApi);
            }
        }

    }
}
