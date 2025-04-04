using HRM.LetStalk.Core.Services.Interfaces;

namespace HRM.LetStalk.Core.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using Constants;
using Models;

public class SendNotificationService(IHttpClientService httpClientService) : ISendNotificationService
{
    public JwtToken GetToken(string baseUrl, string userName, string password)
    {
        var url = $"{baseUrl}{LetStalkApiPath.Auth}?username={userName}&password={password}";
        var token = httpClientService.ExecuteGet<JwtToken>(url);
        return token;
    }

    public async Task<JwtToken> GetTokenAsync(string baseUrl, string userName, string password)
    {
        var url = $"{baseUrl}{LetStalkApiPath.Auth}?username={userName}&password={password}";
        var token = await httpClientService.ExecuteGetAsync<JwtToken>(url);
        return token;
    }

    public void SendNotificationToLetStalk(string baseUrl, string token, Dictionary<string, object> data)
    {
        var url = baseUrl + LetStalkApiPath.Notify;
        httpClientService.ExecutePost<object>(url, token, data);
    }

    public async Task SendNotificationToLetStalkAsync(string baseUrl, string token, Dictionary<string, object> data)
    {
        var url = baseUrl + LetStalkApiPath.Notify;
        await httpClientService.ExecutePostAsync<object>(url, token, data);
    }
}