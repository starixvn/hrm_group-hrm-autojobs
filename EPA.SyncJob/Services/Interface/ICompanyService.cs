using EPA.SyncJob.Model;
using Microsoft.AspNetCore.Http;

namespace EPA.SyncJob.Services.Interfaces;

public interface ICompanyService
{
    Task<CompanySite> GetCompanySiteByDomainAsync();


}