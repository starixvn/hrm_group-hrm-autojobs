namespace EPA.SyncJob.Services
{
    using EPA.SyncJob.Criteria;
    using EPA.SyncJob.Model;
    using EPA.SyncJob.Model.API;
    using EPA.SyncJob.Models;
    using EPA.SyncJob.Services.Interface;
    using EPA.SyncJob.Services.Interfaces;
    using EPA.SyncJob.ThirdPartyAPI;

    public class DisciplinePermissionService(
        IBaseService baseService,
        ITeamDisciplinePermissionAPI teamSupervisorApi,
        IEmployeeDisciplinePermissionAPI employeeSupervisorApi) : IDisciplinePermissionService
    {

        public async Task<IEnumerable<EmployeeDisciplinePermission>> GetEmployeesAsync(DateTime fromDate, short companyId)
        {
            var criteria = new GetEmployeeDisciplinePermissionCriteria { FromDate = fromDate, CompanyId = companyId  };
            var employeeSupervisors = await baseService.GetDataAsync<EmployeeDisciplinePermission>(criteria);

            return employeeSupervisors;
        }

        public async Task<IEnumerable<TeamDisciplinePermission>> GetGroupsSync(DateTime fromDate, short companyId)
        {
            var criteria = new GetGroupDisciplinePermissionCriteria { FromDate = fromDate, CompanyId = companyId};
            var groupSupervisors = await baseService.GetDataAsync<TeamDisciplinePermission>(criteria);

            return groupSupervisors;
        }

        public async Task<CommandResult> UpdateEmployeesAsync(IEnumerable<EmployeeDisciplinePermission> employees, TokenAccess token, ServiceAPI serviceApi)
        {
            return await employeeSupervisorApi.UpdateAsync(employees, token, serviceApi);
        }

        public async Task<CommandResult> UpdateGroupsAsync(IEnumerable<TeamDisciplinePermission> groups, TokenAccess token, ServiceAPI serviceApi)
        {
            return await teamSupervisorApi.UpdateAsync(groups, token, serviceApi);
        }

        public async Task<DateTime> GetLastModifiedDateForEmployeeAsync(TokenAccess token, ServiceAPI serviceApi)
        {
            return await employeeSupervisorApi.GetLastModifiedDateAsync(token, serviceApi);
        }

        public async Task<DateTime> GetLastModifiedDateForGroupAsync(TokenAccess token, ServiceAPI serviceApi)
        {
            return await teamSupervisorApi.GetLastModifiedDateAsync(token, serviceApi);
        }
    }
}