using Microsoft.AspNetCore.Mvc;

namespace JFS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JiraToTfsController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public string Test()
        {
            return "Hello darkness my old friend, I've come to talk with you again";
        }
    }
}