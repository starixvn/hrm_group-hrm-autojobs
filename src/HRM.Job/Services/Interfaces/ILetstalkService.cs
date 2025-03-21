namespace HRM.Job.Services.Interfaces;

public interface ILetStalkService
{
    Task SendNotificationAsync(string letStalkId, string subject, string message);
}