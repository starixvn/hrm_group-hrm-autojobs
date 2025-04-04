

using EPA.SyncJob.ThirdPartyAPI;

namespace EPA.SyncJob.Services
{
    using Criteria;
    using Model;
    using Model.API;
    using Models;
    using Interface;
    using Interfaces;

    public class JobTitleService(IBaseService baseService, IJobTitleAPI jobTitleApi) : IJobTitleService
    {
        public async Task<IEnumerable<JobTitle>> GetJobTitles(short companyId)
        {
            var criteria = new GetJobTitleCriteria { CompanyId = companyId };
            var jobTitles = await baseService.GetDataAsync<JobTitle>(criteria);

            return jobTitles;
        }


        public async Task<CommandResult> UpdateAsync(IEnumerable<JobTitle> jobTitles, TokenAccess token, ServiceAPI serviceApi)
        {
            return await jobTitleApi.UpdateAsync(jobTitles, token, serviceApi);
        }

    }
}