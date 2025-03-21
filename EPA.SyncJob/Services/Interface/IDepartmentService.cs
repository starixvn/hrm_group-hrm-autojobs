using EPA.SyncJob.Model.API;

namespace EPA.SyncJob.Services.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<Department>> GetDepartments(DateTime? fromDate, short companyId);
}