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
            Config config = Config.GetConfig(_context, hook.Issue.Fields.Priority);
            // Validate
            Sync sync = _context.Sync.FirstOrDefault(s => s.JiraKey == hook.Issue.Key);

            if (config == null || sync != null)
                return Ok("Can't create issue");
            // Create
            WorkItem workItem = new WorkItem
            {
                Title = hook.Issue.Fields.Summary,
                ReproSteps = $"{hook.Issue.Fields.Description}\n \nOpend At: {hook.Issue.Fields.Created}\nBy: {hook.User.DisplayName}\nEmail: {hook.User.EmailAddress}",
                CreatedDate = hook.Issue.Fields.Created,
                AreaPath = config.TfsConfig.Area,
                TeamProject = config.TfsConfig.TeamProject,
                IterationPath = config.TfsConfig.Iteration,
                Priority = _context.Priority.First(p => p.JiraPriority == hook.Issue.Fields.Priority).TfsPriority,
                Links = new List<Link>
                {
                    new Link
                    {
                        rel = "System.LinkTypes.Hierarchy-Reverse",
                        url = $"{Env.TFS_URI}_apis/wit/workItems/{config.TfsConfig.ParentId}",
                    }
                }
            };
            var result = await WorkItems.CreateBug(workItem.ToParameterList(), config);  // TODO: Check if successeeded

            Resource TfsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Resource>(result);
            // Create new sync record
            sync = new Sync
            {
                JiraKey = hook.Issue.Key,
                TfsId = TfsResponse.Id,
                Rev = TfsResponse.Rev
            };

            await _context.AddAsync(sync);
            await _context.SaveChangesAsync();

            return Ok(result);
        }

        [HttpPost]
        [Route("issue/[action]")]
        public async Task<IActionResult> Update([FromBody] JiraHook hook)
        {
            // Get configs
            Config config = Config.GetConfig(_context, hook.Issue.Fields.Priority);
            // Validate
            Sync sync = _context.Sync.FirstOrDefault(s => s.JiraKey == hook.Issue.Key);

            if (config == null || sync == null || sync.Deleted)
                return Ok($"Not found. Deleted: {sync.Deleted}");
            // Update
            WorkItem workItem = new WorkItem();

            foreach (var changelogItem in hook.ChangeLog.Items)
            {
                switch (changelogItem.Field)
                {
                    case "description":
                        workItem.ReproSteps = changelogItem.Tostring;
                        break;
                    case "summary":
                        workItem.Title = changelogItem.Tostring;
                        break;
                }
            }

            var result = await WorkItems.UpdateBug(workItem.ToParameterListNotEmptyFields(sync.Rev), config, sync.TfsId);  // TODO: Check if successeeded

            Resource TfsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Resource>(result);

            sync.Rev = TfsResponse.Rev;
            await _context.SaveChangesAsync();

            return Ok(result);
        }

        [HttpPost]
        [Route("issue/[action]")]
        public async Task<IActionResult> Delete([FromBody] JiraHook hook)
        {
            Sync sync = _context.Sync.FirstOrDefault(s => s.JiraKey == hook.Issue.Key);

            if (sync == null || sync.Deleted)
                return Ok($"Not found. Deleted: {sync.Deleted}");

            sync.Deleted = true;
            await _context.SaveChangesAsync();

            return Ok("Deleted");
        }
    }
}
