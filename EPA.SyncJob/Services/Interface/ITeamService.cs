using EPA.SyncJob.Model;
using EPA.SyncJob.Model.API;
using EPA.SyncJob.Models;
using Microsoft.AspNetCore.Http;

namespace EPA.SyncJob.Services.Interface;

public interface ITeamService
{
    Task<IEnumerable<Team>> GetTeams(DateTime? fromDate, short companyId);

    Task<DateTime> GetLastModifiedDateAsync(TokenAccess token);

    Task<CommandResult> UpdateAsync(IEnumerable<Team> teams, TokenAccess token);


}