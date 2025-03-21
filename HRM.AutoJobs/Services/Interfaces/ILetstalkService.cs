namespace HRM.AutoJobs.Services.Interfaces;

public interface ILetStalkService
{
    Task SendNotificationAsync(string letStalkId, string subject, string message);
}