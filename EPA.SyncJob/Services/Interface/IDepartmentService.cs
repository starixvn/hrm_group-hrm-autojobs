using EPA.SyncJob.Model;
using EPA.SyncJob.Model.API;
using EPA.SyncJob.Models;

namespace EPA.SyncJob.Services.Interface;

public interface IJobTitleService
{
    Task<IEnumerable<JobTitle>> GetJobTitles(short companyId);


    Task<CommandResult> UpdateAsync(IEnumerable<JobTitle> jobTitles, TokenAccess token, ServiceAPI serviceApi);
}