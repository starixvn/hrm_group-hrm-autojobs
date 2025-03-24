
namespace EPA.SyncJob.Services
{
    using System.Threading.Tasks;
    using EPA.SyncJob.Services.Interfaces;
    using Model;
    using ThirdPartyAPI;

    public class TokenService(ITokenAPI tokenApi) : ITokenService
    {
        public async Task<TokenAccess> Request()
        {
            return await tokenApi.RequestAsync();
        }
    }
}