using EPA.SyncJob.Constants;
using EPA.SyncJob.Helpers;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using EPA.SyncJob.Services.Interfaces;
using EPA.SyncJob.Workers;
using Microsoft.Extensions.Logging;

namespace EPA.SyncJob.Worker
{
    public class SyncEmployeeDisciplinePermissionWorker : BaseWorker
    {
        private readonly IDisciplinePermissionService disciplineService;
        private readonly ITokenService tokenService;
        private readonly ICompanyService companyService;

        public SyncEmployeeDisciplinePermissionWorker(
            IDisciplinePermissionService disciplineService,
            ITokenService tokenService,
            ICompanyService companyService,
            List<JobSetting> jobSettings,
            ILogger<SyncEmployeeDisciplinePermissionWorker> logfile,
            Fanex.Logging.ILogger logger) : base(logfile, logger)
        {
            
            jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncEmployeeDiscipline);
            this.disciplineService = disciplineService;
            this.companyService = companyService;
            this.tokenService = tokenService;
        }

        public override async Task DoWorkAsync()
        {
            //var token = await tokenService.Request();
            //if (!token.IsValid())
            //{
            //    throw new InvalidOperationException("Invalid token");
            //}
            //var companyData = await companyService.GetCompanySiteByDomainAsync();
            //if (companyData == null)
            //{
            //    throw new InvalidOperationException("Invalid company url");
            //}
            //var fromDate = await disciplineService.GetLastModifiedDateForEmployeeAsync(token);
            //var data= await disciplineService.GetEmployeesAsync(fromDate, companyData.CompanyId);
            //if (data.Any())
            //{
            //    await disciplineService.UpdateEmployeesAsync(data, token);
            //}
        }

    }
}
