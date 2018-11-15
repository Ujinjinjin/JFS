using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using JFS.Clients.TfsClient;
using System.Collections.Generic;
using JFS.Models.TFS.WorkItem;
using JFS.Models.Jira;
using JFS.Models.Db;
using System.Linq;
using JFS.Clients.Constants;
using JFS.Models.Requests.TFS;
using Atlassian.Jira;

namespace JFS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TfsToJiraController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly Jira _jira;

        public TfsToJiraController(ApplicationDbContext context)
        {
            _context = context;
            Env.InitEnvs();
            _jira = Jira.CreateRestClient(Env.JIRA_SERVER_URI, Env.JIRA_USER, Env.JIRA_PASSWORD);
        }

        [HttpPost]
        [Route("issue/[action]")]
        public async Task<IActionResult> Create([FromBody] TfsHook hook)
        {
            var issue = _jira.CreateIssue("JFS");
            issue.Type = hook.Resource.Fields.WorkItemType;
            issue.Priority = Mapper.TfsPriorityToJira(1);  // TODO: Retrieve priority from tfs server
            issue.Summary = hook.Resource.Fields.Title;

            var result = await issue.SaveChangesAsync();
            return Ok(result);
        }

        [HttpPost]
        [Route("issue/[action]")]
        public async Task<IActionResult> Update([FromBody] TfsHook hook)
        {
            return Ok();
        }

        [HttpPost]
        [Route("issue/[action]")]
        public async Task<IActionResult> Delete([FromBody] TfsHook hook)
        {
            return Ok();
        }
    }
}
