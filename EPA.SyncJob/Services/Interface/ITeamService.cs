using EPA.SyncJob.Model;
using EPA.SyncJob.Model.API;
using EPA.SyncJob.Models;

namespace EPA.SyncJob.Services.Interface;

public interface ITeamService
{
    Task<IEnumerable<Team>> GetTeams(DateTime? fromDate, short companyId);

    Task<DateTime> GetLastModifiedDateAsync(TokenAccess token, ServiceAPI serviceApi);

    Task<CommandResult> UpdateAsync(IEnumerable<Team> teams, TokenAccess token, ServiceAPI serviceApi);


}