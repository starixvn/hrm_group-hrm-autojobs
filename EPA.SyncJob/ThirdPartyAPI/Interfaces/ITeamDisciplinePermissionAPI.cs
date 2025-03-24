using EPA.SyncJob.Model.API;
using EPA.SyncJob.Model;
using EPA.SyncJob.Models;

namespace EPA.SyncJob.ThirdPartyAPI;

public interface ITeamDisciplinePermissionAPI
{
    Task<DateTime> GetLastModifiedDateAsync(TokenAccess token);

    Task<CommandResult> UpdateAsync(IEnumerable<TeamDisciplinePermission> groupSupervisors, TokenAccess token);
}
