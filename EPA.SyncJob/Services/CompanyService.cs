namespace EPA.SyncJob.Services
{
    using Criteria;
    using Model;
    using Interfaces;

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