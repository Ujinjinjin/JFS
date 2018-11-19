using JFS.Clients.Constants;
using JFS.Clients.TfsClient;
using JFS.Models.Db;
using JFS.Models.Jira;
using JFS.Models.Requests.TFS;
using JFS.Models.TFS.WorkItem;
using JFS.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JFS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JiraToTfsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JiraToTfsController(ApplicationDbContext context)
        {
            _context = context;
            Env.InitEnvs();
        }

        /// <summary>
        /// Catch Jira hook and create similar work item in Azure DevOps (VSTS/TFS)
        /// </summary>
        [HttpPost]
        [Route("issue/[action]")]
        public async Task<IActionResult> Create([FromBody] JiraHook hook)
        {
            // Get configs
            Config config = await Config.GetConfig(_context, hook.Issue.Fields.Priority.Name);
            // Validate
            Sync sync = _context.Sync.FirstOrDefault(s => s.JiraKey == hook.Issue.Key);

            if (config == null || sync != null)
                return Ok("Can't create issue");
            // Create
            WorkItem workItem = new WorkItem
            {
                Title = hook.Issue.Fields.Summary,
                ReproSteps = JFStringer.ToTfsFormat(hook.Issue.Fields.Description), //\n \nOpend At: {hook.Issue.Fields.Created}\nBy: {hook.User.DisplayName}\nEmail: {hook.User.EmailAddress}",
                CreatedDate = hook.Issue.Fields.Created,
                AreaPath = config.TfsConfig.Area,
                TeamProject = config.TfsConfig.TeamProject,
                IterationPath = config.TfsConfig.Iteration,
                Priority = Priority.ToTfsPriority(_context, hook.Issue.Fields.Priority.Name),
                Links = new List<Link>
                {
                    new Link
                    {
                        rel = "System.LinkTypes.Hierarchy-Reverse",
                        url = $"{Env.TFS_URI}_apis/wit/workItems/{config.TfsConfig.ParentId}",
                    }
                }
            };
            var result = await WorkItems.CreateBug(workItem.ToParameterList(), config);  // TODO: Check if succeeded

            Resource tfsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Resource>(result);
            // Sync
            sync = new Sync
            {
                JiraKey = hook.Issue.Key,
                TfsId = tfsResponse.Id,
                Rev = tfsResponse.Rev,
                Title = workItem.Title,
                Description = JFStringer.ToCommonFormat(workItem.ReproSteps),
                Priority = workItem.Priority
            };

            await _context.AddAsync(sync);
            await _context.SaveChangesAsync();

            return Ok("Created");
        }

        [HttpPost]
        [Route("issue/[action]")]
        public async Task<IActionResult> Update([FromBody] JiraHook hook)
        {
            // Get configs
            Config config = await Config.GetConfig(_context, hook.Issue.Fields.Priority.Name);
            // Validate
            Sync sync = _context.Sync.FirstOrDefault(s => s.JiraKey == hook.Issue.Key);

            if (config == null || sync == null || sync.Deleted ||
                (sync.Title == hook.ChangeLog.Items.FirstOrDefault(ch => ch.Field == "summary")?.Tostring &&
                 sync.Description == JFStringer.ToCommonFormat(hook.ChangeLog.Items.FirstOrDefault(ch => ch.Field == "description")?.Tostring) &&
                 sync.Priority == Priority.ToTfsPriority(_context, hook.ChangeLog.Items.FirstOrDefault(ch => ch.Field == "priority")?.Tostring)))
                return Ok($"Not found");
            // Update
            WorkItem workItem = new WorkItem();

            foreach (var changelogItem in hook.ChangeLog.Items)
            {
                switch (changelogItem.Field)
                {
                    case "description":
                        workItem.ReproSteps = JFStringer.ToTfsFormat(changelogItem.Tostring);
                        sync.Description = changelogItem.Tostring;
                        break;
                    case "summary":
                        workItem.Title = changelogItem.Tostring;
                        sync.Title = changelogItem.Tostring;
                        break;
                    case "priority":
                        workItem.Priority = Priority.ToTfsPriority(_context, changelogItem.Tostring);
                        sync.Priority = workItem.Priority;
                        break;
                }
            }

            var result = await WorkItems.UpdateBug(workItem.ToParameterListNotEmptyFields(sync.Rev), config, sync.TfsId);  // TODO: Check if succeeded

            Resource tfsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Resource>(result);

            // Sync
            sync.Rev = tfsResponse.Rev;

            await _context.SaveChangesAsync();

            return Ok("Updated");
        }

        [HttpPost]
        [Route("issue/[action]")]
        public async Task<IActionResult> Delete([FromBody] JiraHook hook)
        {
            Sync sync = _context.Sync.FirstOrDefault(s => s.JiraKey == hook.Issue.Key);

            if (sync == null || sync.Deleted)
                return Ok($"Not found");

            sync.Deleted = true;
            await _context.SaveChangesAsync();

            return Ok("Deleted");
        }
    }
}
