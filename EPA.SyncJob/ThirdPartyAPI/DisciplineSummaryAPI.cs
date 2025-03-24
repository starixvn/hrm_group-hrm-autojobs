namespace EPA.SyncJob.ThirdPartyAPI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EPA.SyncJob.Model.API;
    using EPA.SyncJob.Models;
    using EPA.SyncJob.Services.Interface;
    using Model;
    using Newtonsoft.Json;

    public class DisciplineSummaryAPI(AppSettings appSettings, IHttpClientService httpClient) : IDisciplineSummaryAPI
    {
        public async Task<DateTime> GetLastModifiedDateAsync(TokenAccess token)
        {
            var result = await httpClient.ExecuteGetAsync<DateTime>(appSettings.ServiceAPI + "DisciplineSummary/GetLastModifiedDate", token.Value, null);
            return result;
        }

        public async Task<CommandResult> UpdateAsync(IEnumerable<DisciplineSummary> disciplines, TokenAccess token)
        {
            var result = await httpClient.ExecutePostAsync<CommandResult>(appSettings.ServiceAPI + "DisciplineSummary", token.Value, 
                JsonConvert.SerializeObject(disciplines.ToArray()));
            return result;
        }
    }
}