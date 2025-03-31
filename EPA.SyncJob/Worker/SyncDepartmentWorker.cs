using EPA.SyncJob.Constants;
using EPA.SyncJob.Helpers;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using EPA.SyncJob.Services.Interfaces;
using EPA.SyncJob.Workers;

namespace EPA.SyncJob.Worker
{
    public class SyncDepartmentWorker : BaseWorker
    {
        private readonly IDepartmentService departmentService;
        private readonly ITokenService tokenService;
        private readonly ICompanyService companyService;
        private readonly List<ServiceAPI> serviceApis;
        private readonly ILogger<SyncDepartmentWorker> logfile;
        private readonly Fanex.Logging.ILogger logger;

        public SyncDepartmentWorker(
            IDepartmentService departmentService,
            ITokenService tokenService,
            ICompanyService companyService,
            List<JobSetting> jobSettings,
            List<ServiceAPI> serviceApis,
            ILogger<SyncDepartmentWorker> logfile,
            Fanex.Logging.ILogger logger) : base(logfile, logger)
        {
            jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncDepartment);
            this.departmentService = departmentService;
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
                    logfile.LogInformation($"Successful SyncDepartment service API: {serviceApi.Name}");
                }
                catch (Exception ex)
                {
                    await logger.ErrorAsync($"Error SyncDepartment service API: {serviceApi.Name} - {ex}");
                    logfile.LogError($"Error SyncDepartment service API: {serviceApi.Name} - {ex}");
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
            var fromDate = await departmentService.GetLastModifiedDateAsync(token, serviceApi);
            var departments = await departmentService.GetDepartments(fromDate, serviceApi.CompanyId);
            if (departments.Any())
            {
                await departmentService.Update(departments, token, serviceApi);
            }
        }
    }
}
