
using EPA.SyncJob.Models;

namespace EPA.SyncJob.Services
{
    using System.Threading.Tasks;
    using EPA.SyncJob.Services.Interfaces;
    using Model;
    using ThirdPartyAPI;

    public class TokenService(ITokenAPI tokenApi) : ITokenService
    {

        public async Task<TokenAccess> Request(ServiceAPI serviceApi)
        {
            return await tokenApi.RequestAsync(serviceApi);
        }
    }
}