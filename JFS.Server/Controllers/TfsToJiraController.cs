using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using JFS.Models.Db;
using System.Linq;
using JFS.Clients.Constants;
using JFS.Models.Requests.TFS;
using Atlassian.Jira;
using System.Net;
using JFS.Models.TFS.WorkItem;
using JFS.Utils;

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
            int priority = int.Parse(hook.Resource.Fields.Priority);
            Config config = await Config.GetConfig(_context, priority);
            // Validate
            Sync sync = _context.Sync.FirstOrDefault(s => s.TfsId == hook.Resource.Id);

            if (config == null || sync != null || config.TfsConfig.Priority != priority)
                return Ok("Can't create issue");


            var issue = _jira.CreateIssue(config.JiraConfig.Project);
            issue.Type = hook.Resource.Fields.WorkItemType;
            issue.Priority = Priority.ToJiraPriority(_context, priority);
            issue.Summary = hook.Resource.Fields.Title;
            issue.Description = JFStringer.ToCommonFormat(hook.Resource.Fields.ReproSteps);

            var result = await issue.SaveChangesAsync();

            // Sync
            sync = new Sync
            {
                JiraKey = result.Key.Value,
                TfsId = hook.Resource.Id,
                Rev = hook.Resource.Rev,
                Title = issue.Summary,
                Description = issue.Description,
                Priority = Priority.ToTfsPriority(_context, issue.Priority.Name)
            };

            await _context.AddAsync(sync);
            await _context.SaveChangesAsync();

            return Ok("Created");
        }

        [HttpPost]
        [Route("issue/[action]")]
        public async Task<IActionResult> Update([FromBody] TfsHook<UpdatedResource> hook)
        {
            int priority = int.Parse(hook.Resource.Revision.Fields.Priority);
            // Get configs
            Config config = await Config.GetConfig(_context, priority);
            // Validate
            Sync sync = _context.Sync.FirstOrDefault(s => s.TfsId == hook.Resource.Revision.Id);

            if (sync == null || sync.Deleted || config.TfsConfig.Priority != priority || sync.Rev == hook.Resource.Revision.Rev ||
                (sync.Title == hook.Resource.Fields.Title?.NewValue &&
                 sync.Description == JFStringer.ToCommonFormat(hook.Resource.Fields.ReproSteps?.NewValue) &&
                 sync.Priority == int.Parse(hook.Resource.Fields.Priority?.NewValue)))
                return Ok($"Not found");
            // Update
            var issue = await _jira.Issues.GetIssueAsync(sync.JiraKey);

            issue.Summary = hook.Resource.Fields.Title != null ? hook.Resource.Fields.Title.NewValue : issue.Summary;
            issue.Description = hook.Resource.Fields.ReproSteps != null ? JFStringer.ToCommonFormat(hook.Resource.Fields.ReproSteps.NewValue) : issue.Description;
            issue.Priority = hook.Resource.Fields.Priority != null
                ? Priority.ToJiraPriority(_context, hook.Resource.Fields.Priority.NewValue)
                : issue.Priority;

            // Sync
            sync.Rev = hook.Resource.Revision.Rev;
            sync.Title = issue.Summary;
            sync.Description = issue.Description;
            sync.Priority = Priority.ToTfsPriority(_context, issue.Priority.Name);

            var result = await issue.SaveChangesAsync();

            await _context.SaveChangesAsync();
            
            return Ok("Updated");
        }

        [HttpPost]
        [Route("issue/[action]")]
        public async Task<IActionResult> Delete([FromBody] TfsHook<Resource> hook)
        {
            Sync sync = _context.Sync.FirstOrDefault(s => s.TfsId == hook.Resource.Id);

            if (sync == null || sync.Deleted)
                return Ok($"Not found");

            sync.Deleted = true;
            await _context.SaveChangesAsync();

            return Ok("Deleted");
        }
    }
}
