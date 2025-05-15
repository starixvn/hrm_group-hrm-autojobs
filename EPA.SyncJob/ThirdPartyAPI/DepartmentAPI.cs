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

public class DepartmentAPI(IHttpClientService httpClient) : IDepartmentAPI
{
    public async Task<DateTime> GetLastModifiedDate(TokenAccess token, ServiceAPI serviceApi)
    {
        var result = await httpClient.ExecuteGetAsync<DateTime>(serviceApi.Domain + "Department", token.Value);
        return result;
    }

    public async Task<CommandResult> UpdateAsync(IEnumerable<Department> departments, TokenAccess token, ServiceAPI serviceApi)
    {
        var result = await httpClient.ExecutePostAsync<CommandResult>(serviceApi.Domain + "Department", token.Value, JsonConvert.SerializeObject(departments.ToArray()));
        return result;
    }
}