using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using JFS.Clients.Constants;
using Newtonsoft.Json;

namespace JFS.Clients.TfsClient
{
    // TODO: Move to distinct nuget package
    public class TfsClient
    {
        private string _credentials;
        private HttpClient _httpClient;

        /// <summary>
        /// Create new Azure DevOps client using ENV vars to retrieve personal access token and workspace uri.
        /// </summary>
        public TfsClient()
        {
            Env.InitEnvs();
            _credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Env.JFS_ACCESS_TOKEN)));
            _httpClient = CreateClient(_credentials);
        }

        /// <summary>
        /// Create HttpClient with passed credentials
        /// </summary>
        public static HttpClient CreateClient(string credentials)
        {
            HttpClient client;

            if (Env.USE_PROXY)
            {
                var handler = new HttpClientHandler();
                handler.DefaultProxyCredentials = CredentialCache.DefaultCredentials;

                client = new HttpClient(handler);
            }
            else
            {
                client = new HttpClient();
            }

            client.BaseAddress = new Uri(Env.JFS_URI);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            return client;
        }

        public async Task<HttpResponseMessage> GetAsync(string uri, string query)
        {
            var response = await _httpClient.GetAsync($"{uri}?{query}");
            //response.EnsureSuccessStatusCode();
            //string responseBody = await response.Content.ReadAsStringAsync();
            //var deserializedResponse = JsonConvert.DeserializeObject(responseBody);
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string uri, string query, T body, string accept="application/json-patch+json")
        {
            string postBody = JsonConvert.SerializeObject(body);
            var response = await _httpClient.PostAsync($"{uri}?{query}", new StringContent(postBody, Encoding.UTF8, accept));
            //response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
