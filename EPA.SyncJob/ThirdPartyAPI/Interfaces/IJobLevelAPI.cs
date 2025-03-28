namespace EPA.SyncJob.ThirdPartyAPI
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model.API;
    using Models;
    using Model;

    public interface IJobLevelAPI
    {
        Task<DateTime> GetLastModifiedDate(TokenAccess token, ServiceAPI serviceApi);

        Task<CommandResult> UpdateAsync(IEnumerable<JobLevel> jobLevel, TokenAccess token, ServiceAPI serviceApi);
    }
}