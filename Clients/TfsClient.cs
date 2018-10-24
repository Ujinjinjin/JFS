using System;
using System.Net.Http;
using System.Net.Http.Headers;
using JFS.Clients.Constants;

namespace JFS.Clients
{
    // TODO: Move to distinct nuget package
    public class TfsClient
    {
        private string _uri;
        private string _personalAccessToken;
        private string _credentials;
        private HttpClient _client;

        /// <summary>
        /// Create new Azure DevOps client using ENV vars to retrieve personal access token and workspace uri.
        /// </summary>
        public TfsClient()
        {
            _uri = Environment.GetEnvironmentVariable(Env.URI);
            if (string.IsNullOrEmpty(_uri))
            {
                throw new ArgumentException($"Environment variable {Env.URI} is not defined. Set env value and try again.");
            }

            _personalAccessToken = Environment.GetEnvironmentVariable(Env.ACCESS_TOKEN);
            if (string.IsNullOrEmpty(_personalAccessToken))
            {
                throw new ArgumentException($"Environment variable {Env.ACCESS_TOKEN} is not defined. Set env value and try again.");
            }

            _credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", _personalAccessToken)));

            // TODO: Move to "public HttpClient CreateClient()"
            _client = new HttpClient { BaseAddress = new Uri(_uri) };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _credentials);
        }

        public HttpResponseMessage RetrieveTasks()
        {
            HttpResponseMessage response = _client.GetAsync("_apis/projects?stateFilter=All&api-version=1.0").Result;
            return response;
        }
    }
}
