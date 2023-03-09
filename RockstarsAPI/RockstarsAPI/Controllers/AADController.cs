using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Client;

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

    }
}
