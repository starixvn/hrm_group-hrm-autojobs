using EPA.SyncJob.Model.API;
using EPA.SyncJob.Model;
using EPA.SyncJob.Models;

namespace EPA.SyncJob.ThirdPartyAPI;

public interface IEmployeeDisciplinePermissionAPI
{
    Task<CommandResult> UpdateAsync(IEnumerable<EmployeeDisciplinePermission> jobTitles, TokenAccess token, ServiceAPI serviceApi);

    Task<DateTime> GetLastModifiedDateAsync(TokenAccess token, ServiceAPI serviceApi);
}
