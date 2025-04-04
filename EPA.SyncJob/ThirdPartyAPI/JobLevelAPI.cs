namespace EPA.SyncJob.ThirdPartyAPI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Model.API;
    using Models;
    using Services.Interface;
    using Model;
    using Newtonsoft.Json;

    public class JobLevelAPI(IHttpClientService httpClient) : IJobLevelAPI
    {
        public async Task<DateTime> GetLastModifiedDate(TokenAccess token, ServiceAPI serviceApi)
        {
            var result = await httpClient.ExecuteGetAsync<DateTime>(serviceApi.Domain + "JobLevel", token.Value, null);
            return result;
        }

        public async Task<CommandResult> UpdateAsync(IEnumerable<JobLevel> jobLevel, TokenAccess token, ServiceAPI serviceApi)
        {
            var result = await httpClient.ExecutePostAsync<CommandResult>(serviceApi.Domain + "JobLevel", token.Value, JsonConvert.SerializeObject(jobLevel.ToArray()));
            return result;
        }
    }
}