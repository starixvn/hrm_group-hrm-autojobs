using EPA.SyncJob.Constants;
using EPA.SyncJob.Helpers;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using EPA.SyncJob.Services.Interfaces;
using EPA.SyncJob.Workers;
using Microsoft.Extensions.Logging;

namespace EPA.SyncJob.Worker
{
    public class SyncDepartmentWorker : BaseWorker
    {
        private readonly IDepartmentService departmentService;
        private readonly ITokenService tokenService;
        private readonly ICompanyService companyService;

        public SyncDepartmentWorker(
            IDepartmentService departmentService,
            ITokenService tokenService,
            ICompanyService companyService,
            List<JobSetting> jobSettings,
            ILogger<SyncDepartmentWorker> logfile,
            Fanex.Logging.ILogger logger) : base(logfile, logger)
        {
            
            jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncDepartment);
            this.departmentService = departmentService;
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
            var fromDate = await departmentService.GetLastModifiedDateAsync(token);
            var departments= await departmentService.GetDepartments(fromDate, companyData.CompanyId);
            if (departments.Any())
            {
                await departmentService.Update(departments, token);
            }
        }

    }
}
