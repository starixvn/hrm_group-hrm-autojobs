

using EPA.SyncJob.ThirdPartyAPI;

namespace EPA.SyncJob.Services
{
    using EPA.SyncJob.Criteria;
    using EPA.SyncJob.Model;
    using EPA.SyncJob.Model.API;
    using EPA.SyncJob.Models;
    using EPA.SyncJob.Services.Interface;
    using EPA.SyncJob.Services.Interfaces;

    public class DepartmentService(IBaseService baseService, IDepartmentAPI departmentApi) : IDepartmentService
    {
        public async Task<IEnumerable<Department>> GetDepartments(DateTime? fromDate, short companyId)
        {
            var criteria = new GetDepartmentCriteria { FromDate = fromDate, CompanyId = companyId };
            var departments = await baseService.GetDataAsync<Department>(criteria);

            return departments;
        }

        public async Task<DateTime> GetLastModifiedDateAsync(TokenAccess token)
        {
            return await departmentApi.GetLastModifiedDate(token);
        }

        public async Task<CommandResult> Update(IEnumerable<Department> departments, TokenAccess token)
        {
            return await departmentApi.UpdateAsync(departments, token);
        }

       


    }
}