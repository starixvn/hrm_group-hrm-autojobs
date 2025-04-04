using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.LetStalk.Core.Models;

namespace HRM.LetStalk.Core.Services.Interfaces;

public interface ISendNotificationService
{
    JwtToken GetToken(string baseUrl, string userName, string password);

    Task<JwtToken> GetTokenAsync(string baseUrl, string userName, string password);

    void SendNotificationToLetStalk(string baseUrl, string token, Dictionary<string, object> data);

    Task SendNotificationToLetStalkAsync(string baseUrl, string token, Dictionary<string, object> data);
}