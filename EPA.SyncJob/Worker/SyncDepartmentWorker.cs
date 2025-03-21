using System.Resources;
using EPA.SyncJob.Constants;
using EPA.SyncJob.Helpers;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interfaces;
using EPA.SyncJob.Workers;
using Microsoft.Extensions.Logging;

namespace EPA.SyncJob.Worker
{
    public class SyncDepartmentWorker : BaseWorker
    {
        private readonly IDepartmentService departmentService;

        public SyncDepartmentWorker(
            IDepartmentService departmentService,
            List<JobSetting> jobSettings,
            ILogger<SyncDepartmentWorker> logfile,
            Fanex.Logging.ILogger logger) : base(logfile, logger)
        {
            jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncDepartment);
            this.departmentService = departmentService;
        }

        public override async Task DoWorkAsync()
        {
            var test = await departmentService.GetDepartments(null, 3);
            var test123 = "123";
        }
    }
}
