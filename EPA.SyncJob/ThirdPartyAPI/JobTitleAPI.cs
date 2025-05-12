using EPA.SyncJob.Model;
using EPA.SyncJob.Model.API;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using Newtonsoft.Json;

namespace EPA.SyncJob.ThirdPartyAPI;

public class JobTitleAPI(IHttpClientService httpClient) : IJobTitleAPI
{
    public async Task<CommandResult> UpdateAsync(IEnumerable<JobTitle> jobTitles, TokenAccess token, ServiceAPI serviceApi)
    {
        var result = await httpClient.ExecutePostAsync<CommandResult>(serviceApi.Domain + "JobTitle",
            token.Value, JsonConvert.SerializeObject(jobTitles.ToArray()));

        return result;
    }
}