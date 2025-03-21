

namespace EPA.SyncJob.Services
{
    using EPA.SyncJob.Criteria;
    using EPA.SyncJob.Model.API;
    using EPA.SyncJob.Services.Interfaces;

    public class DepartmentService : IDepartmentService
    {
        private readonly IBaseService baseService;

        public DepartmentService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<IEnumerable<Department>> GetDepartments(DateTime? fromDate, short companyId)
        {
            var criteria = new GetDepartmentCriteria { FromDate = fromDate, CompanyId = companyId };
            var departments = await baseService.GetDataAsync<Department>(criteria);

            return departments;
        }


    }
}