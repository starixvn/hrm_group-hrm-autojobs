namespace EPA.SyncJob.Services
{
    using Criteria;
    using Model;
    using Model.API;
    using Models;
    using Interface;
    using Interfaces;
    using ThirdPartyAPI;

    public class EmployeeService(IBaseService baseService, IEmployeeAPI employeeApi) : IEmployeeService
    {

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(DateTime? fromDate, short companyId)
        {
            var criteria = new GetEmployeeCriteria { FromDate = fromDate, CompanyId = companyId };
            var employees = await baseService.GetDataAsync<Employee>(criteria);

            return employees;
        }

        public async Task<DateTime> GetLastModifiedDateAsync(TokenAccess token, ServiceAPI serviceApi)
        {
            return await employeeApi.GetLastModifiedDateAsync(token, serviceApi);
        }

        public async Task<CommandResult> UpdateAsync(IEnumerable<Employee> employees, TokenAccess token, ServiceAPI serviceApi)
        {
            return await employeeApi.UpdateAsync(employees, token, serviceApi);
        }
    }
}