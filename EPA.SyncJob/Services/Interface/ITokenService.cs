using EPA.SyncJob.Model;
using EPA.SyncJob.Models;

namespace EPA.SyncJob.Services.Interfaces;

public interface ITokenService
{
    Task<TokenAccess> Request(ServiceAPI serviceApi);
}