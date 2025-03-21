namespace HRM.AutoJobs.Models;

public class LetStalkSettings
{
    public int BotId { get; set; }
    public string BaseUrl { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string TestingAccountId { get; set; }
    public bool Enable { get; set; }
}