using EPA.SyncJob.Model;
using EPA.SyncJob.Model.API;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using Newtonsoft.Json;

namespace EPA.SyncJob.ThirdPartyAPI;

public class TeamDisciplinePermissionAPI(IHttpClientService httpClient) : ITeamDisciplinePermissionAPI
{
    public async Task<DateTime> GetLastModifiedDateAsync(TokenAccess token, ServiceAPI serviceApi)
    {
        var result = await httpClient.ExecuteGetAsync<DateTime>(serviceApi.Domain + "TeamDisciplinePermission", token.Value, null);

        return result;
    }

    public async Task<CommandResult> UpdateAsync(IEnumerable<TeamDisciplinePermission> groupSupervisors, TokenAccess token, ServiceAPI serviceApi)
    {
        var result = await httpClient.ExecutePostAsync<CommandResult>(serviceApi.Domain + "TeamDisciplinePermission",
            token.Value, JsonConvert.SerializeObject(groupSupervisors.ToArray()));

        return result;
    }
}