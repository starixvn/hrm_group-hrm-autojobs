using System.Resources;
using EPA.SyncJob.Constants;
using EPA.SyncJob.Helpers;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services;
using EPA.SyncJob.Services.Interfaces;
using EPA.SyncJob.Workers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace EPA.SyncJob.Worker
{
    public class SyncDepartmentWorker : BaseWorker
    {
        private readonly IDepartmentService departmentService;
        private readonly ITokenService tokenService;

        public SyncDepartmentWorker(
            IDepartmentService departmentService,
            ITokenService tokenService,
            List<JobSetting> jobSettings,
            ILogger<SyncDepartmentWorker> logfile,
            Fanex.Logging.ILogger logger) : base(logfile, logger)
        {
            
            jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncDepartment);
            this.departmentService = departmentService;
            this.tokenService = tokenService;
        }

        public override async Task DoWorkAsync()
        {
            var token = await tokenService.Request();
            if (!token.IsValid())
            {
                throw new InvalidOperationException("Invalid token");
            }
            var fromDate = await departmentService.GetLastModifiedDateAsync(token);
            var departments= await departmentService.GetDepartments(null, 3);
            if (departments.Any())
            {
                await departmentService.Update(departments, token);
            }
        }
    }
}
