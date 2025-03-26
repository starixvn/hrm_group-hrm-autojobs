namespace EPA.SyncJob.Services
{
    using EPA.SyncJob.Criteria;
    using EPA.SyncJob.Model;
    using EPA.SyncJob.Model.API;
    using EPA.SyncJob.Models;
    using EPA.SyncJob.Services.Interface;
    using EPA.SyncJob.Services.Interfaces;
    using EPA.SyncJob.ThirdPartyAPI;

    public class TeamService(IBaseService baseService, ITeamAPI teamApi) : ITeamService
    {

        public async Task<IEnumerable<Team>> GetTeams(DateTime? fromDate, short companyId)
        {
            var criteria = new GetTeamCriteria { FromDate = fromDate, CompanyId = companyId};
            var teams = await baseService.GetDataAsync<Team>(criteria);

            return teams;
        }

        public async Task<DateTime> GetLastModifiedDateAsync(TokenAccess token, ServiceAPI serviceApi)
        {
            return await teamApi.GetLastModifiedDate(token, serviceApi);
        }

        public async Task<CommandResult> UpdateAsync(IEnumerable<Team> teams, TokenAccess token, ServiceAPI serviceApi)
        {
            return await teamApi.UpdateAsync(teams, token, serviceApi);
        }
    }
}