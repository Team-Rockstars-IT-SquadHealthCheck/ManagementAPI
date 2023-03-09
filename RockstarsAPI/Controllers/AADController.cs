using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Throw an exception with the error message
                    throw new Exception($"API call failed with status code {response.StatusCode}.");
                }
            }
        }


        [HttpGet]
        [Route("/users")]
        public async Task<string> getusers()
        {
            //https://developer.microsoft.com/en-us/graph/graph-explorer
            //HttpContext.Response.Headers.Add("Content-Type", "application/json");
            string apiUrl = "https://graph.microsoft.com/v1.0/users/";
            string bearerToken = "eyJ0eXAiOiJKV1QiLCJub25jZSI6ImVGQkEwZXJnRUpULTA0ZWNFbGQ0ZHFwdmJlMzZoa182QnhLZUJWYUF0WTgiLCJhbGciOiJSUzI1NiIsIng1dCI6Ii1LSTNROW5OUjdiUm9meG1lWm9YcWJIWkdldyIsImtpZCI6Ii1LSTNROW5OUjdiUm9meG1lWm9YcWJIWkdldyJ9.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTAwMDAtYzAwMC0wMDAwMDAwMDAwMDAiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8yZDY0YWYzYS1jM2RhLTQzMDctYjdlNi1jYzQ5MmJiMWNiZDEvIiwiaWF0IjoxNjc4MzY5NjY5LCJuYmYiOjE2NzgzNjk2NjksImV4cCI6MTY3ODM3NDk3MywiYWNjdCI6MCwiYWNyIjoiMSIsImFpbyI6IkFWUUFxLzhUQUFBQWNKSGMyT0w0Rk1oYkNjdU1vVnZvOXovZ1VZSDM0QzJEenFLMmFWRFdTbmppNEI4NkRWZmJ4L1k3NkhVWUJrRGd3WG5sQVVvZEExcEdBNG1SblNvWmRHTmtRb0tUdm5JRER3TGZBalZzVVcwPSIsImFtciI6WyJwd2QiLCJtZmEiXSwiYXBwX2Rpc3BsYXluYW1lIjoiR3JhcGggRXhwbG9yZXIiLCJhcHBpZCI6ImRlOGJjOGI1LWQ5ZjktNDhiMS1hOGFkLWI3NDhkYTcyNTA2NCIsImFwcGlkYWNyIjoiMCIsImZhbWlseV9uYW1lIjoidmFuIExlZXV3ZW4iLCJnaXZlbl9uYW1lIjoiUm9zZSIsImlkdHlwIjoidXNlciIsImlwYWRkciI6IjE0NS45My4xMDQuMTc3IiwibmFtZSI6IlJvc2UgdmFuIExlZXV3ZW4iLCJvaWQiOiIyYTdjOTFkMS03NWQzLTRiNjYtODVlZS1hOTk3ZTk5NzEwYTAiLCJwbGF0ZiI6IjMiLCJwdWlkIjoiMTAwMzIwMDI3RUY3NzEwQSIsInJoIjoiMC5BVTRBT3E5a0xkckRCME8zNXN4Sks3SEwwUU1BQUFBQUFBQUF3QUFBQUFBQUFBQ0RBTG8uIiwic2NwIjoiRGlyZWN0b3J5LlJlYWQuQWxsIG9wZW5pZCBwcm9maWxlIFVzZXIuUmVhZCBlbWFpbCIsInN1YiI6IjRCLTM0MW1nRHU4TlhRWnN5dThqSnJmMzh0MHpKNENfeTJ0OWR3OWtWSTQiLCJ0ZW5hbnRfcmVnaW9uX3Njb3BlIjoiRVUiLCJ0aWQiOiIyZDY0YWYzYS1jM2RhLTQzMDctYjdlNi1jYzQ5MmJiMWNiZDEiLCJ1bmlxdWVfbmFtZSI6ImFkbWluQDI2enJkNy5vbm1pY3Jvc29mdC5jb20iLCJ1cG4iOiJhZG1pbkAyNnpyZDcub25taWNyb3NvZnQuY29tIiwidXRpIjoiekZFZXEtcWZOVXlBY3RGY1d1eVFBQSIsInZlciI6IjEuMCIsIndpZHMiOlsiNjJlOTAzOTQtNjlmNS00MjM3LTkxOTAtMDEyMTc3MTQ1ZTEwIiwiYjc5ZmJmNGQtM2VmOS00Njg5LTgxNDMtNzZiMTk0ZTg1NTA5Il0sInhtc19zdCI6eyJzdWIiOiJlbUo0SWRaZ2N2OUozcEtPbUJSRTgxR2FfTHRwSGN3ck56Y3I3eFBmbTFVIn0sInhtc190Y2R0IjoxNjc3NzY1NjU1LCJ4bXNfdGRiciI6IkVVIn0.MegrpHirZNpAtJwR87zbzxP0-2vzdHdnNQR9Z6kVffxfS7x-fjR2ss-5f7XEvYnGyrkHoof0v0ki_Q3zWM92m6sk4XjUzguF2wHwqvFKjETwpoTswIcDKHoZcfR2r2cSj0f8TLMs1HJjtY-0-LAbu4jlrMG6ab7atroN6we5O2SXsRTiwJl1BZfcvowSRx8QDXJhTPIkuHhCucOrNEhYm9Y4rjK67c_Eb6HPV_5eKYjY4Sk0ECK_JaBD63jBYq_TYTUpnZcmmDUlZLieSyPNHjofK7gYu86HpnESRucD5S4ppl2DXCCrP66lpaLvFSX8eMe4699sC65tYALQRbpZhQ";

            string apiResponse = await CallApiWithBearerToken(apiUrl, bearerToken);
            Console.WriteLine(apiResponse);
            return apiResponse;
            
        }
    }
}
