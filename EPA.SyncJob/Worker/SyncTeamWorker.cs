using EPA.SyncJob.Constants;
using EPA.SyncJob.Helpers;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using EPA.SyncJob.Services.Interfaces;
using EPA.SyncJob.Workers;
using Microsoft.Extensions.Logging;

namespace EPA.SyncJob.Worker
{
    public class SyncTeamWorker : BaseWorker
    {
        private readonly ITeamService teamService;
        private readonly ITokenService tokenService;
        private readonly ICompanyService companyService;

        public SyncTeamWorker(
            ITeamService teamService,
            ITokenService tokenService,
            ICompanyService companyService,
            List<JobSetting> jobSettings,
            ILogger<SyncTeamWorker> logfile,
            Fanex.Logging.ILogger logger) : base(logfile, logger)
        {
            
            jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncTeam);
            this.teamService = teamService;
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
            var fromDate = await teamService.GetLastModifiedDateAsync(token);
            var teams= await teamService.GetTeams(fromDate, companyData.CompanyId);
            if (teams.Any())
            {
                await teamService.UpdateAsync(teams, token);
            }
        }

    }
}
