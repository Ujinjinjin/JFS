using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using JFS.Models.Db;
using System.Linq;
using JFS.Clients.Constants;
using JFS.Models.Requests.TFS;
using Atlassian.Jira;
using System.Net;

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
            //_jira.RestClient.RestSharpClient.Proxy = WebRequest.GetSystemWebProxy();
            //_jira.RestClient.RestSharpClient.Proxy.Credentials = CredentialCache.DefaultCredentials;
        }

        [HttpPost]
        [Route("issue/[action]")]
        public async Task<IActionResult> Create([FromBody] TfsHook<Resource> hook)
        {
            // Get configs
            Config config = Config.GetConfig(_context, 1);
            // Validate
            Sync sync = _context.Sync.FirstOrDefault(s => s.TfsId == hook.Resource.Id);

            if (config == null && sync != null) // || config.TfsConfig.Priority != 1)
                return Ok("Can't create issue");

            var issue = _jira.CreateIssue("JFS");
            issue.Type = hook.Resource.Fields.WorkItemType;
            issue.Priority = _context.Priority.First(p => p.TfsPriority == 1).JiraPriority;  // TODO: Retrieve priority and description from tfs server
            issue.Summary = hook.Resource.Fields.Title;
            // TODO: Retrieve bug description from TFS

            var result = await issue.SaveChangesAsync();

            // Create new sync record
            sync = new Sync
            {
                JiraKey = result.Key.Value,
                TfsId = hook.Resource.Id,
                Rev = hook.Resource.Rev
            };

            await _context.AddAsync(sync);
            await _context.SaveChangesAsync();

            return Ok(result);
        }

        [HttpPost]
        [Route("issue/[action]")]
        public async Task<IActionResult> Update([FromBody] TfsHook<UpdatedResource> hook)
        {
            // Get configs
            Config config = Config.GetConfig(_context);
            // Validate
            Sync sync = _context.Sync.FirstOrDefault(s => s.TfsId == hook.Resource.Revision.Id);

            if (sync == null || sync.Deleted) // || config.TfsConfig.Priority != 1)
                return Ok($"Not found. Deleted: {sync.Deleted}");
            // Update
            var issue = await _jira.Issues.GetIssueAsync(sync.JiraKey);

            issue.Summary = hook.Resource.Fields.Title != null ? hook.Resource.Fields.Title.NewValue : issue.Summary;
            issue.Description = hook.Resource.Fields.ReproSteps != null ? hook.Resource.Fields.ReproSteps.NewValue : issue.Description;

            var result = await issue.SaveChangesAsync();

            sync.Rev = hook.Resource.Revision.Rev;

            await _context.SaveChangesAsync();
            
            return Ok(result);
        }

        [HttpPost]
        [Route("issue/[action]")]
        public async Task<IActionResult> Delete([FromBody] TfsHook<Resource> hook)
        {
            Sync sync = _context.Sync.FirstOrDefault(s => s.TfsId == hook.Resource.Id);

            if (sync == null || sync.Deleted)
                return Ok($"Not found. Deleted: {sync.Deleted}");

            sync.Deleted = true;
            await _context.SaveChangesAsync();

            return Ok("Deleted");
        }
    }
}
