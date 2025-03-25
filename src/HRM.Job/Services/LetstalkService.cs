using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Job.Models;
using HRM.Job.Services.Interfaces;
using HRM.LetStalk.Core.Services.Interfaces;

namespace HRM.Job.Services;

public class LetStalkService(
    LetStalkSettings letStalkSettings,
    ISendNotificationService sendNotificationService)
    : ILetStalkService
{
    private const string TemplateContent = "<strong>{0}</strong><br /> {1}";

    public async Task SendNotificationAsync(string letStalkId, string subject, string message)
    {
        var letStalk = !string.IsNullOrWhiteSpace(letStalkSettings.TestingAccountId) ? letStalkSettings.TestingAccountId : letStalkId;
        if (letStalkSettings.Enable && !string.IsNullOrWhiteSpace(letStalk))
        {
            var jwtToken = await sendNotificationService.GetTokenAsync(letStalkSettings.BaseUrl, letStalkSettings.Username, letStalkSettings.Password);
            if (jwtToken != null)
            {
                var content = string.Format(TemplateContent, subject, message);
                var data = new Dictionary<string, object>
                {
                    { "BotId", letStalkSettings.BotId },
                    { "ConversationId", letStalk },
                    { "Message", content }
                };

                await sendNotificationService.SendNotificationToLetStalkAsync(letStalkSettings.BaseUrl, jwtToken.Token, data);
            }
        }
    }
}