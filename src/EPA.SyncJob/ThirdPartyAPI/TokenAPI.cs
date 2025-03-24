

using EPA.SyncJob.Helpers;

namespace EPA.SyncJob.ThirdPartyAPI
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using EPA.SyncJob.Models;
    using EPA.SyncJob.Model;
    using Newtonsoft.Json;

    public class TokenAPI : ITokenAPI
    {
        private readonly AppSettings appSettings;
        private readonly IHttpClientFactory httpClientFactory;

        public TokenAPI(AppSettings appSettings, IHttpClientFactory httpClientFactory)
        {
            this.appSettings = appSettings;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<TokenAccess> RequestAsync()
        {
            using var client = httpClientFactory.CreateClient();

            var form = new Dictionary<string, string>
                {
                    { "grant_type", appSettings.GrantType },
                    { "username", appSettings.ApplicationId },
                    { "password", appSettings.ApplicationId }
                };

            using (var encodedContent = new FormUrlEncodedContent(form))
            {
                try
                {
                    var tokenResponse = await client.PostAsync(new Uri(appSettings.ServiceAPI + "token"), encodedContent);
                    tokenResponse.EnsureSuccessStatusCode();
                    var jsonResponse = await tokenResponse.Content.ReadAsStringAsync();
                    var result =  JsonConvert.DeserializeObject<TokenAccess>(jsonResponse);

                    return result;
                }
                catch (HttpRequestException e)
                {
                    // Log the exception or handle it as needed
                    throw new Exception("Error requesting token", e);
                }
            }
        }
    }
}