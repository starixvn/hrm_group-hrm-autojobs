using EPA.SyncJob.Model;
using EPA.SyncJob.Model.API;
using EPA.SyncJob.Models;

namespace EPA.SyncJob.Services.Interface;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetEmployeesAsync(DateTime? fromDate, short companyId);

    Task<DateTime> GetLastModifiedDateAsync(TokenAccess token, ServiceAPI serviceApi);

    Task<CommandResult> UpdateAsync(IEnumerable<Employee> employees, TokenAccess token, ServiceAPI serviceApi);


}