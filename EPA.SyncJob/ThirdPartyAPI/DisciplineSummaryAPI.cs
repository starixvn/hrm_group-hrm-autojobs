namespace EPA.SyncJob.ThirdPartyAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;
using Model.API;
using Models;
using Newtonsoft.Json;
using Services.Interface;

public class DisciplineSummaryAPI(IHttpClientService httpClient) : IDisciplineSummaryAPI
{
    public async Task<DateTime> GetLastModifiedDateAsync(TokenAccess token, ServiceAPI serviceApi)
    {
        var result = await httpClient.ExecuteGetAsync<DateTime>(serviceApi.Domain + "DisciplineSummary/GetLastModifiedDate", token.Value);
        return result;
    }

    public async Task<CommandResult> UpdateAsync(IEnumerable<DisciplineSummary> disciplines, TokenAccess token, ServiceAPI serviceApi)
    {
        var result = await httpClient.ExecutePostAsync<CommandResult>(serviceApi.Domain + "DisciplineSummary", token.Value,
            JsonConvert.SerializeObject(disciplines.ToArray()));
        return result;
    }
}