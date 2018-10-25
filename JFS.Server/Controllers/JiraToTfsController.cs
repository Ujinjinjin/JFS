using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using JFS.Clients.TfsClient;
using System.Collections.Generic;
using JFS.Models.TFS.WorkItem;

namespace JFS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JiraToTfsController : ControllerBase
    {
        [HttpPost]
        [Route("issue/[action]")]
        public async Task<IActionResult> Create()
        {
            WorkItem workItem = new WorkItem
            {
                Title = "Auto Created Work Item from code",
                AreaPath = "JFS",
                TeamProject = "JFS",
                IterationPath = "JFS\\Iteration 1",
                Priority = 1,
                Links = new List<Link>
                {
                    new Link
                    {
                        rel = "System.LinkTypes.Hierarchy-Reverse",
                        url = "https://dev.azure.com/kagallad/_apis/wit/workItems/2",
                    }
                }
            };
            var result = await WorkItems.CreateWorkItem(workItem.ToParameterList());
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Test()
        {
            var result = await WorkItems.TestApi();
            return Ok(result);
        }
    }
}
