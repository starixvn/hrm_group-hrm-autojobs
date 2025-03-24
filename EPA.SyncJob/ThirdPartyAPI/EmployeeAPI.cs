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

    public class EmployeeAPI(AppSettings appSettings, IHttpClientService httpClient) : IEmployeeAPI
    {
        public async Task<DateTime> GetLastModifiedDateAsync(TokenAccess token)
        {
            var result = await httpClient.ExecuteGetAsync<DateTime>(appSettings.ServiceAPI + "Employee", token.Value, null);
            return result;
        }

        public async Task<CommandResult> UpdateAsync(IEnumerable<Employee> employees, TokenAccess token)
        {
            var result = await httpClient.ExecutePostAsync<CommandResult>(appSettings.ServiceAPI + "Employee", token.Value, JsonConvert.SerializeObject(employees.ToArray()));
            return result;
        }
    }
}