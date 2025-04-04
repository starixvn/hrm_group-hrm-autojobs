namespace EPA.SyncJob.Services
{
    using Criteria;
    using Model;
    using Model.API;
    using Models;
    using Interface;
    using Interfaces;
    using ThirdPartyAPI;

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

        public async Task<DateTime> GetLastModifiedDateAsync(TokenAccess token, ServiceAPI serviceApi)
        {
            return await disciplineSummaryApi.GetLastModifiedDateAsync(token, serviceApi);
        }

        public async Task<CommandResult> UpdateAsync(IEnumerable<DisciplineSummary> disciplines, TokenAccess token, ServiceAPI serviceApi)
        {
            return await disciplineSummaryApi.UpdateAsync(disciplines, token, serviceApi);
        }
    }
}