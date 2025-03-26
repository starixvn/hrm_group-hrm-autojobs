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

    public class EmployeeAPI(IHttpClientService httpClient) : IEmployeeAPI
    {
        public async Task<DateTime> GetLastModifiedDateAsync(TokenAccess token, ServiceAPI serviceApi)
        {
            var result = await httpClient.ExecuteGetAsync<DateTime>(serviceApi.Domain + "Employee", token.Value, null);
            return result;
        }

        public async Task<CommandResult> UpdateAsync(IEnumerable<Employee> employees, TokenAccess token, ServiceAPI serviceApi)
        {
            var result = await httpClient.ExecutePostAsync<CommandResult>(serviceApi.Domain + "Employee", token.Value, JsonConvert.SerializeObject(employees.ToArray()));
            return result;
        }
    }
}