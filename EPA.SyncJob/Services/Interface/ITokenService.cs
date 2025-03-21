using EPA.SyncJob.Model;

namespace EPA.SyncJob.Services.Interfaces;

public interface ITokenService
{
    Task<TokenAccess> Request();
}