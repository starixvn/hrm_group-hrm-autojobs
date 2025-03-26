using EPA.SyncJob.Models;

namespace EPA.SyncJob.Services
{
    using EPA.SyncJob.Criteria;
    using EPA.SyncJob.Model;
    using EPA.SyncJob.Services.Interfaces;
    using Microsoft.AspNetCore.Http;

    public class CompanyService(IBaseService baseService) : ICompanyService
    {
        public async Task<CompanySite> GetCompanySiteByDomainAsync(string domain)
        {
            var criteria = new GetCompanySiteByDomainCriteria
            {
                Domain = domain.TrimEnd('/')
            };
            return await baseService.GetDataEntityAsync<CompanySite>(criteria);
        }




    }
}