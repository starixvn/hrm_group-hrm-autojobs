using EPA.SyncJob.Constants;
using EPA.SyncJob.Helpers;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using EPA.SyncJob.Services.Interfaces;
using EPA.SyncJob.Workers;

namespace EPA.SyncJob.Worker
{
    public class SyncTeamWorker : BaseWorker
    {
        private readonly ITeamService teamService;
        private readonly ITokenService tokenService;
        private readonly ICompanyService companyService;
        private readonly List<ServiceAPI> serviceApis;
        private readonly ILogger<SyncTeamWorker> logfile;
        private readonly Fanex.Logging.ILogger logger;

        public SyncTeamWorker(
            ITeamService teamService,
            ITokenService tokenService,
            ICompanyService companyService,
            List<JobSetting> jobSettings,
            List<ServiceAPI> serviceApis,
            ILogger<SyncTeamWorker> logfile,
            Fanex.Logging.ILogger logger) : base(logfile, logger)
        {

            jobSetting = JobHelper.GetJobConfig(jobSettings, JobSettingConstants.SyncTeam);
            this.teamService = teamService;
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
                    logfile.LogInformation($"Successful SyncTeam service API: {serviceApi.Name}");
                }
                catch (Exception ex)
                {
                    await logger.ErrorAsync($"Error SyncTeam service API: {serviceApi.Name} - {ex}");
                    logfile.LogError($"Error SyncTeam service API: {serviceApi.Name} - {ex}");
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

            var fromDate = await teamService.GetLastModifiedDateAsync(token, serviceApi);
            var teams = await teamService.GetTeams(fromDate, serviceApi.CompanyId);
            if (teams.Any())
            {
                await teamService.UpdateAsync(teams, token, serviceApi);
            }
        }
    }
}
