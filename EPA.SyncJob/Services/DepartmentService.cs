

using EPA.SyncJob.ThirdPartyAPI;

namespace EPA.SyncJob.Services
{
    using Criteria;
    using Model;
    using Model.API;
    using Models;
    using Interface;
    using Interfaces;

    public class DepartmentService(IBaseService baseService, IDepartmentAPI departmentApi) : IDepartmentService
    {
        public async Task<IEnumerable<Department>> GetDepartments(DateTime? fromDate, short companyId)
        {
            var criteria = new GetDepartmentCriteria { FromDate = fromDate, CompanyId = companyId };
            var departments = await baseService.GetDataAsync<Department>(criteria);

            return departments;
        }

        public async Task<DateTime> GetLastModifiedDateAsync(TokenAccess token, ServiceAPI serviceApi)
        {
            return await departmentApi.GetLastModifiedDate(token, serviceApi);
        }

        public async Task<CommandResult> Update(IEnumerable<Department> departments, TokenAccess token, ServiceAPI serviceApi)
        {
            return await departmentApi.UpdateAsync(departments, token, serviceApi);
        }


    }
}