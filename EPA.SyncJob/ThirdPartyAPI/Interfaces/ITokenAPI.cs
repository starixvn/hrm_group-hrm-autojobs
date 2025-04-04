using EPA.SyncJob.Models;

namespace EPA.SyncJob.ThirdPartyAPI
{
    using System.Threading.Tasks;
    using Model;

    public interface ITokenAPI
    {
        Task<TokenAccess> RequestAsync(ServiceAPI serviceApi);
    }
}