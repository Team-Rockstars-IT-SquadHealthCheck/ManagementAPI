using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Graph.Models;

namespace RockstarsAPI.models
{
    public class AAD
    {
        public int AADId { get; set; }
        public string AADDisplayName { get; set; }
        public string AADGivenName { get; set; }
        public string AADSurName { get; set; }
        public string AADMail { get; set; }
        public string UserPrincipalName { get; set; }
        
    }


    public class BearerTokenProvider
    {
        private readonly HttpClient _httpClient;

        public BearerTokenProvider(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> GetBearerTokenAsync(string tenantId, string clientId, string username, string password, string clientsecret)
        {
            //var tenantId = "2d64af3a-c3da-4307-b7e6-cc492bb1cbd1";
            //var clientId = "91a1472e-d799-4ee1-b625-62351b81da68";
            //var clientSecret = "WMD8Q~Km~ruxA0wQNnWuy980_TbTCooetz0Z4b4O";
            var resource = "https://graph.microsoft.com/";

            var content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("scope", $"{resource}/.default"),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password),
            new KeyValuePair<string, string>("client_secret", clientsecret)
        });

            var tokenUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

            using var client = new HttpClient();
            var response = await client.PostAsync(tokenUrl, content);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(json);

            var accessToken = result.GetProperty("access_token").GetString();
            accessToken = accessToken;
            return accessToken;
        }
    

        //private class TokenResponse
        //{
        //    [JsonPropertyName("access_token")]
        //    public string AccessToken { get; set; }
        //}
    }
}
