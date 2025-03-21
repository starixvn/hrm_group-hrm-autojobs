using HRM.Job.Models.MailDeliver;

namespace HRM.Job.Services.Interfaces;

public interface IBaseMailService
{
    Task<bool> SendEmailAsync(MailRecord email);
}