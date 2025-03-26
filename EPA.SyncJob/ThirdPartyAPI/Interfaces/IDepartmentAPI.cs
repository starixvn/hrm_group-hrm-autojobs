namespace EPA.SyncJob.ThirdPartyAPI
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EPA.SyncJob.Model.API;
    using EPA.SyncJob.Models;
    using Model;

    public interface IDepartmentAPI
    {
        Task<DateTime> GetLastModifiedDate(TokenAccess token, ServiceAPI serviceApi);

        Task<CommandResult> UpdateAsync(IEnumerable<Department> departments, TokenAccess token, ServiceAPI serviceApi);
    }
}