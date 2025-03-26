

using EPA.SyncJob.ThirdPartyAPI;

namespace EPA.SyncJob.Services
{
    using EPA.SyncJob.Criteria;
    using EPA.SyncJob.Model;
    using EPA.SyncJob.Model.API;
    using EPA.SyncJob.Models;
    using EPA.SyncJob.Services.Interface;
    using EPA.SyncJob.Services.Interfaces;

    public class JobLevelService(IBaseService baseService, IJobLevelAPI jobLevelApi) : IJobLevelService
    {
        public async Task<IEnumerable<JobLevel>> GetJobLevelAsync(DateTime? fromDate)
        {
            var criteria = new GetJobLevelCriteria { FromDate = fromDate };
            var jobLevel = await baseService.GetDataAsync<JobLevel>(criteria);

            return jobLevel;
        }

        public async Task<DateTime> GetLastModifiedDateAsync(TokenAccess token, ServiceAPI serviceApi)
        {
            return await jobLevelApi.GetLastModifiedDate(token, serviceApi);
        }

        public Task<CommandResult> UpdateAsync(IEnumerable<JobLevel> jobLevel, TokenAccess token, ServiceAPI serviceApi)
        {
            return jobLevelApi.UpdateAsync(jobLevel, token, serviceApi);
        }

    }
}