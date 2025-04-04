namespace EPA.SyncJob.Models
{
    public class ServiceAPI
    {
        public string Name { get; set; }
        public string ApplicationId { get; set; }
        public string Domain { get; set; }
        public short CompanyId { get; set; }
        public string GrantType { get; } = "password";
    }
}
