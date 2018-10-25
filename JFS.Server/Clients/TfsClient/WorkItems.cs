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

        public static async Task<string> CreateWorkItem(List<WorkItemParameter> parameters)
        {
            HttpResponseMessage response = await _client.PostAsync($"{Env.JFS_PROJECT}/_apis/wit/workitems/$Bug", "api-version=4.1", parameters);
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        public static async Task<string> TestApi()
        {
            //Query query = new Query
            //{
            //    query = "Select [System.Id] From WorkItems Where [System.WorkItemType] = 'Task'"
            //};
            //string postBody = Newtonsoft.Json.JsonConvert.SerializeObject(query);
            //HttpResponseMessage response = await _client.PostAsync($"{Env.JFS_PROJECT}/{Env.JFS_TEAM}/_apis/wit/wiql?api-version=4.1", new StringContent(postBody, Encoding.UTF8, "application/json"));
            HttpResponseMessage response = await _client.GetAsync($"_apis/wit/workitems/2", "api-version=4.1");
            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }
    }
}
