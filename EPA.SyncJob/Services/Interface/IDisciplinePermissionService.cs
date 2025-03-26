using EPA.SyncJob.Model;
using EPA.SyncJob.Model.API;
using EPA.SyncJob.Models;

namespace EPA.SyncJob.Services.Interface;

public interface IDisciplinePermissionService
{
    Task<IEnumerable<EmployeeDisciplinePermission>> GetEmployeesAsync(DateTime fromDate, short companyId);


    Task<IEnumerable<TeamDisciplinePermission>> GetGroupsSync(DateTime fromDate, short companyId);

    Task<CommandResult> UpdateEmployeesAsync(IEnumerable<EmployeeDisciplinePermission> employees, TokenAccess token, ServiceAPI serviceApi);

    Task<CommandResult> UpdateGroupsAsync(IEnumerable<TeamDisciplinePermission> groups, TokenAccess token, ServiceAPI serviceApi);

    Task<DateTime> GetLastModifiedDateForEmployeeAsync(TokenAccess token, ServiceAPI serviceApi);

    Task<DateTime> GetLastModifiedDateForGroupAsync(TokenAccess token, ServiceAPI serviceApi);
}