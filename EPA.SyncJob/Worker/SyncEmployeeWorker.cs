using EPA.SyncJob.Constants;
using EPA.SyncJob.Helpers;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using EPA.SyncJob.Services.Interfaces;
using EPA.SyncJob.Workers;
using Microsoft.Extensions.Logging;

namespace EPA.SyncJob.Worker
{
    public class SyncEmployeeWorker : BaseWorker
    {
        private readonly IEmployeeService employeeService;
        private readonly ITokenService tokenService;
        private readonly ICompanyService companyService;
        private readonly List<ServiceAPI> serviceApis;
        private readonly ILogger<SyncEmployeeWorker> logfile;

        public SyncEmployeeWorker(
            IEmployeeService employeeService,
            ITokenService tokenService,
            ICompanyService companyService,
            List<JobSetting> jobSettings,
            List<ServiceAPI> serviceApis,
            ILogger<SyncEmployeeWorker> logfile,
            Fanex.Logging.ILogger logger) : base(logfile, logger)
        {

            jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncEmployee);
            this.employeeService = employeeService;
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
                    logfile.LogInformation($"Successful SyncEmployee service API: {serviceApi.Name}");
                }
                catch (Exception ex)
                {
                    logfile.LogError($"Error SyncEmployee service API: {serviceApi.Name} - {ex}");
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
            var fromDate = await employeeService.GetLastModifiedDateAsync(token, serviceApi);
            var data = await employeeService.GetEmployeesAsync(fromDate, companyData.CompanyId);
            if (data.Any())
            {
                await employeeService.UpdateAsync(data, token, serviceApi);
            }

        }
    }
}
