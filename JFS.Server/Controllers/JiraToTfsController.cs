using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using JFS.Clients.TfsClient;
using System.Collections.Generic;
using JFS.Models.TFS.WorkItem;
using JFS.Models.Jira;
using JFS.Models.Db;
using System.Linq;

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
        }

        [HttpPost]
        [Route("issue/[action]")]
        public async Task<IActionResult> Create([FromBody] JiraHook hook)
        {
            Sync checkSync = _context.Sync.FirstOrDefault(s => s.JiraId == int.Parse(hook.Issue.Id));

            if (checkSync != null)
                return Ok("Already exist");

            WorkItem workItem = new WorkItem
            {
                Title = hook.Issue.Fields.Summary,
                ReproSteps = hook.Issue.Fields.Description, // $"{hook.Issue.Fields.Description}\n\nOpend At: {hook.Issue.Fields.Created}\nBy: {hook.User.DisplayName}\nEmail: {hook.User.EmailAddress}",
                CreatedDate = hook.Issue.Fields.Created,
                AreaPath = "JFS",  // From config
                TeamProject = "JFS",  // From config
                IterationPath = "JFS\\Iteration 1", // From config
                Priority = 1,  // From mapping
                Links = new List<Link>
                {
                    new Link
                    {
                        rel = "System.LinkTypes.Hierarchy-Reverse",  // From config
                        url = "https://dev.azure.com/kagallad/_apis/wit/workItems/2",
                    }
                }
            };
            var result = await WorkItems.CreateBug(workItem.ToParameterList());

            BlankWI TfsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<BlankWI>(result);  // Do beatifull

            Sync sync = new Sync
            {
                JiraId = int.Parse(hook.Issue.Id),
                TfsId = TfsResponse.Id,
                Rev = 1
            };

            await _context.AddAsync(sync);
            await _context.SaveChangesAsync();

            return Ok(result);
        }

        [HttpPost]
        [Route("issue/[action]")]
        public async Task<IActionResult> Update([FromBody] JiraHook hook)
        {
            Sync sync = _context.Sync.FirstOrDefault(s => s.JiraId == int.Parse(hook.Issue.Id));

            if (sync == null)
                return Ok("Not found");

            int tfsId = sync.TfsId;

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

            var result = await WorkItems.UpdateBug(workItem.ToParameterListNotEmptyFields(sync.Rev), tfsId);

            sync.Rev += 1;
            await _context.SaveChangesAsync();

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
