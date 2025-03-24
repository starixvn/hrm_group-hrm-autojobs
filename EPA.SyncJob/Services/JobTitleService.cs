

using EPA.SyncJob.ThirdPartyAPI;

namespace EPA.SyncJob.Services
{
    using EPA.SyncJob.Criteria;
    using EPA.SyncJob.Model;
    using EPA.SyncJob.Model.API;
    using EPA.SyncJob.Models;
    using EPA.SyncJob.Services.Interface;
    using EPA.SyncJob.Services.Interfaces;

    public class JobTitleService(IBaseService baseService, IJobTitleAPI jobTitleApi) : IJobTitleService
    {
        public async Task<IEnumerable<JobTitle>> GetJobTitles(short companyId)
        {
            var criteria = new GetJobTitleCriteria { CompanyId = companyId };
            var jobTitles = await baseService.GetDataAsync<JobTitle>(criteria);

            return jobTitles;
        }


        public async Task<CommandResult> UpdateAsync(IEnumerable<JobTitle> jobTitles, TokenAccess token)
        {
            return await jobTitleApi.UpdateAsync(jobTitles, token);
        }

    }
}