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

    public class EmployeeDisciplinePermissionAPI(IHttpClientService httpClient) : IEmployeeDisciplinePermissionAPI
    {
        public async Task<DateTime> GetLastModifiedDateAsync(TokenAccess token, ServiceAPI serviceApi)
        {
            var result = await httpClient.ExecuteGetAsync<DateTime>(serviceApi.Domain + "EmployeeDisciplinePermission", token.Value, null);
            return result;
        }

        public async Task<CommandResult> UpdateAsync(IEnumerable<EmployeeDisciplinePermission> employeeSupervisors, TokenAccess token, ServiceAPI serviceApi)
        {
            var result = await httpClient.ExecutePostAsync<CommandResult>(serviceApi.Domain + "EmployeeDisciplinePermission", token.Value,
                JsonConvert.SerializeObject(employeeSupervisors.ToArray()));
            return result;
        }
    }
}