namespace EPA.SyncJob.Services
{
    using EPA.SyncJob.Criteria;
    using EPA.SyncJob.Model;
    using EPA.SyncJob.Model.API;
    using EPA.SyncJob.Models;
    using EPA.SyncJob.Services.Interface;
    using EPA.SyncJob.Services.Interfaces;
    using EPA.SyncJob.ThirdPartyAPI;

    public class DisciplineSummaryService(IBaseService baseService, IDisciplineSummaryAPI disciplineSummaryApi) : IDisciplineSummaryService
    {

        public async Task<IEnumerable<DisciplineSummary>> GetDisciplineSummaryAsync(DateTime lastModifiedDate, short companyId)
        {
            var criteria = new GetDisciplineSummaryCriteria
            {
                LastModifiedDate = lastModifiedDate,
                CompanyId = companyId
            };

            var disciplines = await baseService.GetDataAsync<DisciplineSummary>(criteria);

            return disciplines;
        }

        public async Task<DateTime> GetLastModifiedDateAsync(TokenAccess token)
        {
            return await disciplineSummaryApi.GetLastModifiedDateAsync(token);
        }

        public async Task<CommandResult> UpdateAsync(IEnumerable<DisciplineSummary> disciplines, TokenAccess token)
        {
            return await disciplineSummaryApi.UpdateAsync(disciplines, token);
        }
    }
}