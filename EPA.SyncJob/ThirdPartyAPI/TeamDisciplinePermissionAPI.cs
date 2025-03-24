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
    
    public class TeamDisciplinePermissionAPI(AppSettings appSettings, IHttpClientService httpClient) : ITeamDisciplinePermissionAPI
    {
        public async Task<DateTime> GetLastModifiedDateAsync(TokenAccess token)
        {
            var result = await httpClient.ExecuteGetAsync<DateTime>(appSettings.ServiceAPI + "TeamDisciplinePermission", token.Value, null);

            return result;
        }

        public async Task<CommandResult> UpdateAsync(IEnumerable<TeamDisciplinePermission> groupSupervisors, TokenAccess token)
        {
            var result = await httpClient.ExecutePostAsync<CommandResult>(appSettings.ServiceAPI + "TeamDisciplinePermission",
                token.Value, JsonConvert.SerializeObject(groupSupervisors.ToArray()));

            return result;
        }
    }
}