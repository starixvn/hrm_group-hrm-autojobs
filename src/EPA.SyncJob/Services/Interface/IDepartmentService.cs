using EPA.SyncJob.Model;
using EPA.SyncJob.Model.API;
using EPA.SyncJob.Models;

namespace EPA.SyncJob.Services.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<Department>> GetDepartments(DateTime? fromDate, short companyId);

    Task<DateTime> GetLastModifiedDateAsync(TokenAccess token);

    Task<CommandResult> Update(IEnumerable<Department> departments, TokenAccess token);
}