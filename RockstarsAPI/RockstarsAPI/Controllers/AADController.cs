using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Kiota.Abstractions;
using RockstarsAPI.models;

namespace RockstarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AADController : ControllerBase
    {
        //public List<AAD> GetUsers()
        //{
        //    var graphClient = new GraphServiceClient(requestAdapter);
        //}
    }
}
