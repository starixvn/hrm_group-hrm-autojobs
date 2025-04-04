using EPA.SyncJob.Model.API;
using EPA.SyncJob.Model;
using EPA.SyncJob.Models;

namespace EPA.SyncJob.ThirdPartyAPI;

public interface ITeamDisciplinePermissionAPI
{
    Task<DateTime> GetLastModifiedDateAsync(TokenAccess token, ServiceAPI serviceApi);

    Task<CommandResult> UpdateAsync(IEnumerable<TeamDisciplinePermission> groupSupervisors, TokenAccess token, ServiceAPI serviceApi);
}
