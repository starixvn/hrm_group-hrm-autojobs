using EPA.SyncJob.Models;

namespace EPA.SyncJob.Services
{
    using EPA.SyncJob.Criteria;
    using EPA.SyncJob.Model;
    using EPA.SyncJob.Services.Interfaces;
    using Microsoft.AspNetCore.Http;

    public class CompanyService(IBaseService baseService, AppSettings appSettings) : ICompanyService
    {
        public async Task<CompanySite> GetCompanySiteByDomainAsync()
        {
            var criteria = new GetCompanySiteByDomainCriteria
            {
                Domain = appSettings.ServiceAPI.TrimEnd('/')
            };
            return await baseService.GetDataEntityAsync<CompanySite>(criteria);
        }




    }
}