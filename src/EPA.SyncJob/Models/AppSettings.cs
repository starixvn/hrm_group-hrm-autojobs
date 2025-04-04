namespace EPA.SyncJob.Models
{
    public class AppSettings
    {
        public bool IsWriteLog { get; set; }
        public string ServiceAPI { get; set; }
        public string ApplicationId { get; set; }
        public string GrantType { get; } = "password";
    }
}
