namespace EPA.SyncJob.Model
{
    public class ApplicationInfo
    {
        public string GrantType { get; } = "password";

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}