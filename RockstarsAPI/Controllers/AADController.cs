using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RockstarsAPI.models;
using System.Net.Http;
using System.Net.Http.Headers;


namespace RockstarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AADController : ControllerBase
    {
        public readonly IConfiguration _Configuration;
        public AADController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }
        public static async Task<string> CallApiWithBearerToken(string apiUrl, string bearerToken)
        {
            using (var httpClient = new HttpClient())
            {
                // Set the authorization header with the bearer token
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                // Make the API call and get the response
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                // Check if the API call was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as string and return it
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Throw an exception with the error message
                    throw new Exception($"API call failed with status code {response.StatusCode}.");
                }
            }
        }
        //https://developer.microsoft.com/en-us/graph/graph-explorer

        [HttpGet]
        [Route("/AADusers")]
        public async Task<string> getAADusers()
        {
            HttpClient httpClient = new HttpClient();
            var bearerTokenProvider = new BearerTokenProvider(httpClient);
            string tenantid = _Configuration.GetConnectionString("TenantId");
            string ClientId = _Configuration.GetConnectionString("ClientId");
            string clientsecret = _Configuration.GetConnectionString("ClientSecret");
            string username = _Configuration.GetConnectionString("username");
            string password = _Configuration.GetConnectionString("password");
            var bearerToken = await bearerTokenProvider.GetBearerTokenAsync(tenantid, ClientId, username, password, clientsecret);

            string apiUrl = "https://graph.microsoft.com/v1.0/users/";

            string apiResponse = await CallApiWithBearerToken(apiUrl, bearerToken);

            Console.WriteLine(apiResponse);
            
            return apiResponse;
            
        }
    }
}
