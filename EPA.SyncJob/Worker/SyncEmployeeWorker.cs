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

        public SyncEmployeeWorker(
            IEmployeeService employeeService,
            ITokenService tokenService,
            ICompanyService companyService,
            List<JobSetting> jobSettings,
            ILogger<SyncEmployeeWorker> logfile,
            Fanex.Logging.ILogger logger) : base(logfile, logger)
        {
            
            jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncEmployee);
            this.employeeService = employeeService;
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
            var fromDate = await employeeService.GetLastModifiedDateAsync(token);
            var data= await employeeService.GetEmployeesAsync(fromDate, companyData.CompanyId);
            if (data.Any())
            {
                await employeeService.UpdateAsync(data, token);
            }
        }

    }
}
