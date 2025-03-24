namespace EPA.SyncJob.Model
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    public class TokenAccess
    {
        [JsonProperty("access_token")]
        public string Value { get; set; }

        [JsonProperty("token_type")]
        public string Type { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty(".issued")]
        public string Issued { get; set; }

        [JsonProperty(".expires")]
        public string Expires { get; set; }

        [JsonIgnore]
        public DateTime ExpireDate =>
            string.IsNullOrWhiteSpace(Expires)
                ? DateTime.MinValue
                : DateTime.ParseExact(Expires, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);


        public bool IsValid()
        {
            return DateTime.UtcNow < ExpireDate;
        }
    }
}