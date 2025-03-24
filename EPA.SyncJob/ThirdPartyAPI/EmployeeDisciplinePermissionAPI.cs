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

    public class EmployeeDisciplinePermissionAPI(AppSettings appSettings, IHttpClientService httpClient) : IEmployeeDisciplinePermissionAPI
    {
        public async Task<DateTime> GetLastModifiedDateAsync(TokenAccess token)
        {
            var result = await httpClient.ExecuteGetAsync<DateTime>(appSettings.ServiceAPI + "EmployeeDisciplinePermission", token.Value, null);
            return result;
        }

        public async Task<CommandResult> UpdateAsync(IEnumerable<EmployeeDisciplinePermission> employeeSupervisors, TokenAccess token)
        {
            var result = await httpClient.ExecutePostAsync<CommandResult>(appSettings.ServiceAPI + "EmployeeDisciplinePermission", token.Value,
                JsonConvert.SerializeObject(employeeSupervisors.ToArray()));
            return result;
        }
    }
}