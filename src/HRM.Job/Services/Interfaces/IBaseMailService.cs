using System.Threading.Tasks;
using HRM.Job.Models;
using HRM.Job.Models.MailDeliver;

namespace HRM.Job.Services.Interfaces;

public interface IBaseMailService
{
    Task<bool> SendEmailAsync(MailRecord email);

    Task<MailConfig> GetMailConfigAsync(short companyId);

    Task<MailSetting> GetMailSettingByIdAsync(int appId);
}