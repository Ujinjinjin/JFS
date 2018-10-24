using Microsoft.AspNetCore.Mvc;
using JFS.Clients;
using System.Net.Http;

namespace JFS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JiraToTfsController : ControllerBase
    {
        private TfsClient _tfsClient;

        public JiraToTfsController()
        {
            _tfsClient = new TfsClient();
        }

        [HttpGet]
        [Route("[action]")]
        public HttpResponseMessage Test()
        {
            var result = _tfsClient.RetrieveTasks();
            //return "Hello darkness my old friend";
            return result;
        }
    }
}
