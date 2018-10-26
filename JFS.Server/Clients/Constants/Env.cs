using System;

namespace JFS.Clients.Constants
{
    public static class EnvNames
    {
        public const string JFS_ACCESS_TOKEN = "JFS_AZURE_DEVOPS_ACCESS_TOKEN";
        public const string JFS_URI = "JFS_AZURE_DEVOPS_URI";
        public const string JFS_PROJECT = "JFS_AZURE_DEVOPS_PROJECT";
        public const string JFS_TEAM = "JFS_AZURE_DEVOPS_TEAM";
        public const string USE_PROXY = "JFS_USE_PROXY";
    }

    public static class Env
    {
        public static string JFS_ACCESS_TOKEN;
        public static string JFS_URI;
        public static string JFS_PROJECT;
        public static string JFS_TEAM;
        public static bool USE_PROXY;

        public static void InitEnvs()
        {
            string useDefaultProxy;

            JFS_URI = Environment.GetEnvironmentVariable(EnvNames.JFS_URI);
            JFS_ACCESS_TOKEN = Environment.GetEnvironmentVariable(EnvNames.JFS_ACCESS_TOKEN);
            JFS_PROJECT = Environment.GetEnvironmentVariable(EnvNames.JFS_PROJECT);
            JFS_TEAM = Environment.GetEnvironmentVariable(EnvNames.JFS_TEAM);

            AssertEnvs();

            useDefaultProxy = Environment.GetEnvironmentVariable(EnvNames.USE_PROXY);
            USE_PROXY = (useDefaultProxy == "1" || useDefaultProxy == "True");
        }

        private static void AssertEnvs()
        {
            if (string.IsNullOrEmpty(JFS_ACCESS_TOKEN.Trim()) || 
                string.IsNullOrEmpty(JFS_URI.Trim()) ||
                string.IsNullOrEmpty(JFS_PROJECT.Trim()) ||
                string.IsNullOrEmpty(JFS_TEAM.Trim()))
            {
                throw new ApplicationException(
                    "One of required environment variables has no value.\n" +
                    "List of required variables:\n" +
                    "   JFS_AZURE_DEVOPS_ACCESS_TOKEN: Azure DevOps or TFS personal access token. Learn how to acquire it https://docs.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate?view=vsts \n" +
                    "   JFS_AZURE_DEVOPS_URI: URI of your Azure DevOps or TFS organization page e.g. 'https://dev.azure.com/{org_name}/'\n" +
                    "   JFS_AZURE_DEVOPS_PROJECT: Id of Azure DevOps or TFS project. (Will be removed soon)\n" +
                    "   JFS_AZURE_DEVOPS_TEAM: Id of your Azure DevOps or TFS team. (Will be removed soon)\n" +
                    "List of optional variables:\n" +
                    "   JFS_USE_PROXY: if it is true, server will use default system proxy settings. In most cases you don't need this.");
            }
        }
    }
}
