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

public class EmployeeDisciplinePermissionAPI(IHttpClientService httpClient) : IEmployeeDisciplinePermissionAPI
{
    public async Task<DateTime> GetLastModifiedDateAsync(TokenAccess token, ServiceAPI serviceApi)
    {
        var result = await httpClient.ExecuteGetAsync<DateTime>(serviceApi.Domain + "EmployeeDisciplinePermission", token.Value);
        return result;
    }

    public async Task<CommandResult> UpdateAsync(IEnumerable<EmployeeDisciplinePermission> disciplines, TokenAccess token, ServiceAPI serviceApi)
    {
        var result = await httpClient.ExecutePostAsync<CommandResult>(serviceApi.Domain + "EmployeeDisciplinePermission", token.Value,
            JsonConvert.SerializeObject(disciplines.ToArray()));
        return result;
    }
}