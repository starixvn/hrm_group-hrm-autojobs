using EPA.SyncJob.Model.API;
using EPA.SyncJob.Model;
using EPA.SyncJob.Models;

namespace EPA.SyncJob.ThirdPartyAPI;

public interface ITeamAPI
{
    Task<DateTime> GetLastModifiedDate(TokenAccess token);

    Task<CommandResult> UpdateAsync(IEnumerable<Team> teams, TokenAccess token);
}
