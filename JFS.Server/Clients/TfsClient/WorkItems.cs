using JFS.Clients.Constants;
using JFS.Models.Db;
using JFS.Models.TFS.WorkItem;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JFS.Clients.TfsClient
{
    public class WorkItems
    {
        private static TfsClient _client = new TfsClient();

        public static async Task<string> CreateBug(List<WorkItemParameter> parameters, Config config)
        {
            HttpResponseMessage response = await _client.PostAsync($"{config.TfsConfig.TeamProject}/_apis/wit/workitems/$Bug", "api-version=4.1", parameters);
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        public static async Task<string> UpdateBug(List<WorkItemParameter> parameters, Config config, int id)
        {
            HttpResponseMessage response = await _client.PatchAsync($"{config.TfsConfig.TeamProject}/_apis/wit/workitems/{id}", "api-version=4.1", parameters);
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
