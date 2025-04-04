using EPA.SyncJob.Model.API;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using EPA.SyncJob.Model;
using Newtonsoft.Json;

namespace EPA.SyncJob.ThirdPartyAPI
{
    
    public class TeamAPI(IHttpClientService httpClient) : ITeamAPI
    {
        public async Task<DateTime> GetLastModifiedDate(TokenAccess token, ServiceAPI serviceApi)
        {
            var result = await httpClient.ExecuteGetAsync<DateTime>(serviceApi.Domain + "Team", token.Value, null);

            return result;
        }

        public async Task<CommandResult> UpdateAsync(IEnumerable<Team> teams, TokenAccess token, ServiceAPI serviceApi)
        {
            var result = await httpClient.ExecutePostAsync<CommandResult>(serviceApi.Domain + "Team",
                token.Value, JsonConvert.SerializeObject(teams.ToArray()));

            return result;
        }
    }
}