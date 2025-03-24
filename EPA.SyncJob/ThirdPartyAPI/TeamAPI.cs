using EPA.SyncJob.Model.API;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPA.SyncJob.Model;
using Newtonsoft.Json;

namespace EPA.SyncJob.ThirdPartyAPI
{
    
    public class TeamAPI(AppSettings appSettings, IHttpClientService httpClient) : ITeamAPI
    {
        public async Task<DateTime> GetLastModifiedDate(TokenAccess token)
        {
            var result = await httpClient.ExecuteGetAsync<DateTime>(appSettings.ServiceAPI + "Team", token.Value, null);

            return result;
        }

        public async Task<CommandResult> UpdateAsync(IEnumerable<Team> teams, TokenAccess token)
        {
            var result = await httpClient.ExecutePostAsync<CommandResult>(appSettings.ServiceAPI + "Team",
                token.Value, JsonConvert.SerializeObject(teams.ToArray()));

            return result;
        }
    }
}