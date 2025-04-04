

namespace EPA.SyncJob.ThirdPartyAPI
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Models;
    using Model;
    using Newtonsoft.Json;

    public class TokenAPI : ITokenAPI
    {
        private readonly IHttpClientFactory httpClientFactory;

        public TokenAPI(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<TokenAccess> RequestAsync(ServiceAPI serviceApi)
        {
            var handler = new HttpClientHandler { ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator };
            var client = new HttpClient(handler);

            var form = new Dictionary<string, string>
                {
                    { "grant_type", serviceApi.GrantType },
                    { "username", serviceApi.ApplicationId },
                    { "password", serviceApi.ApplicationId }
                };

            using (var encodedContent = new FormUrlEncodedContent(form))
            {
                try
                {
                    var tokenResponse = await client.PostAsync(new Uri(serviceApi.Domain + "token"), encodedContent);
                    tokenResponse.EnsureSuccessStatusCode();
                    var jsonResponse = await tokenResponse.Content.ReadAsStringAsync();
                    var result =  JsonConvert.DeserializeObject<TokenAccess>(jsonResponse);

                    return result;
                }
                catch (HttpRequestException e)
                {
                    // Log the exception or handle it as needed
                    throw new Exception($"Error requesting token API: {serviceApi.Domain}", e);
                }
            }
        }
    }
}