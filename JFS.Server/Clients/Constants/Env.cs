using System;

namespace JFS.Clients.Constants
{
    public static class EnvNames
    {
        public const string TFS_ACCESS_TOKEN = "JFS_AZURE_DEVOPS_ACCESS_TOKEN";
        public const string TFS_URI = "JFS_AZURE_DEVOPS_URI";
        public const string USE_PROXY = "JFS_USE_PROXY";
    }

    public static class Env
    {
        public static string TFS_ACCESS_TOKEN;
        public static string TFS_URI;
        public static bool USE_PROXY;

        public static void InitEnvs()
        {
            string useDefaultProxy;

            TFS_URI = Environment.GetEnvironmentVariable(EnvNames.TFS_URI);
            TFS_ACCESS_TOKEN = Environment.GetEnvironmentVariable(EnvNames.TFS_ACCESS_TOKEN);

            AssertEnvs();

            useDefaultProxy = Environment.GetEnvironmentVariable(EnvNames.USE_PROXY);
            USE_PROXY = (useDefaultProxy == "1" || useDefaultProxy == "True");
        }

        private static void AssertEnvs()
        {
            if (string.IsNullOrEmpty(TFS_ACCESS_TOKEN.Trim()) || 
                string.IsNullOrEmpty(TFS_URI.Trim()))
            {
                throw new ApplicationException(
                    "One of required environment variables has no value.\n" +
                    "List of required variables:\n" +
                    "   JFS_AZURE_DEVOPS_ACCESS_TOKEN: Azure DevOps or TFS personal access token. Learn how to acquire it https://docs.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate?view=vsts \n" +
                    "   JFS_AZURE_DEVOPS_URI: URI of your Azure DevOps or TFS organization page e.g. 'https://dev.azure.com/{org_name}/'\n" +
                    "List of optional variables:\n" +
                    "   JFS_USE_PROXY: if it is true, server will use default system proxy settings. In most cases you don't need this.");
            }
        }
    }
}
