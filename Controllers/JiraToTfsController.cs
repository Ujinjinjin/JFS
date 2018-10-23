using Microsoft.AspNetCore.Mvc;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services;
using Microsoft.VisualStudio.Services.WebApi;

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
