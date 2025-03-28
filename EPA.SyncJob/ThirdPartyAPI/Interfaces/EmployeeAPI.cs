namespace EPA.SyncJob.ThirdPartyAPI
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model.API;
    using Models;
    using Model;

    public interface IEmployeeAPI
    {
        Task<DateTime> GetLastModifiedDateAsync(TokenAccess token, ServiceAPI serviceApi);

        Task<CommandResult> UpdateAsync(IEnumerable<Employee> employees, TokenAccess token, ServiceAPI serviceApi);
    }
}