using EPA.SyncJob.Model;
using EPA.SyncJob.Model.API;
using EPA.SyncJob.Models;

namespace EPA.SyncJob.Services.Interface;

public interface IJobLevelService
{
    Task<IEnumerable<JobLevel>> GetJobLevelAsync(DateTime? fromDate);

    Task<DateTime> GetLastModifiedDateAsync(TokenAccess token);

    Task<CommandResult> UpdateAsync(IEnumerable<JobLevel> jobLevel, TokenAccess token);
}