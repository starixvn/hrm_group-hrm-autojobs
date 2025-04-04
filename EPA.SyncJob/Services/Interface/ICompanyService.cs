using EPA.SyncJob.Model;

namespace EPA.SyncJob.Services.Interfaces;

public interface ICompanyService
{
    Task<CompanySite> GetCompanySiteByDomainAsync(string domain);


}