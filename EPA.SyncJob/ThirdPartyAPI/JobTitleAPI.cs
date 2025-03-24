using EPA.SyncJob.Model;
using EPA.SyncJob.Model.API;
using EPA.SyncJob.Models;
using EPA.SyncJob.Services.Interface;
using Newtonsoft.Json;

namespace EPA.SyncJob.ThirdPartyAPI
{
    public class JobTitleAPI(AppSettings appSettings, IHttpClientService httpClient) : IJobTitleAPI
    {

        public async Task<CommandResult> UpdateAsync(IEnumerable<JobTitle> jobTitles, TokenAccess token)
        {
            var result = await httpClient.ExecutePostAsync<CommandResult>(appSettings.ServiceAPI + "JobTitle",
                token.Value, JsonConvert.SerializeObject(jobTitles.ToArray()));

            return result;
        }
    }
}