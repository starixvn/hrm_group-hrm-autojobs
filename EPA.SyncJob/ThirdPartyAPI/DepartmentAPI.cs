namespace EPA.SyncJob.ThirdPartyAPI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using EPA.SyncJob.Model.API;
    using EPA.SyncJob.Models;
    using EPA.SyncJob.Services.Interface;
    using Model;
    using Newtonsoft.Json;

    public class DepartmentAPI : IDepartmentAPI
    {
        private readonly AppSettings appSettings;
        private readonly IHttpClientService httpClient;

        public DepartmentAPI(AppSettings appSettings, IHttpClientService httpClient)
        {
            this.appSettings = appSettings;
            this.httpClient = httpClient;
        }
        public async Task<DateTime> GetLastModifiedDate(TokenAccess token)
        {
            var result = await httpClient.ExecuteGetAsync<DateTime>(appSettings.ServiceAPI + "Department", token.Value, null);
            return result;
        }

        public async Task<CommandResult> UpdateAsync(IEnumerable<Department> departments, TokenAccess token)
        {
            var result = httpClient.ExecutePostAsync<CommandResult>(appSettings.ServiceAPI + "Department", token.Value, JsonConvert.SerializeObject(departments.ToArray()));
            return result.Result;
        }
    }
}