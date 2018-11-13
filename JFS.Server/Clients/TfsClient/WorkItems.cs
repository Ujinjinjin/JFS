using JFS.Clients.Constants;
using JFS.Models.TFS.WorkItem;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JFS.Clients.TfsClient
{
    public class WorkItems
    {
        private static TfsClient _client = new TfsClient();

        public static async Task<string> CreateBug(List<WorkItemParameter> parameters)
        {
            HttpResponseMessage response = await _client.PostAsync($"{Env.JFS_PROJECT}/_apis/wit/workitems/$Bug", "api-version=4.1", parameters);
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        public static async Task<string> UpdateBug(List<WorkItemParameter> parameters, int id)
        {
            HttpResponseMessage response = await _client.PatchAsync($"{Env.JFS_PROJECT}/_apis/wit/workitems/{id}", "api-version=4.1", parameters);
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
