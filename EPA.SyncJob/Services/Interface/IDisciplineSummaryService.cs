using EPA.SyncJob.Model;
using EPA.SyncJob.Model.API;
using EPA.SyncJob.Models;

namespace EPA.SyncJob.Services.Interface;

public interface IDisciplineSummaryService
{
    Task<IEnumerable<DisciplineSummary>> GetDisciplineSummaryAsync(DateTime lastModifiedDate, short companyId);

    Task<DateTime> GetLastModifiedDateAsync(TokenAccess token, ServiceAPI serviceApi);

    Task<CommandResult> UpdateAsync(IEnumerable<DisciplineSummary> disciplines, TokenAccess token, ServiceAPI serviceApi);


}