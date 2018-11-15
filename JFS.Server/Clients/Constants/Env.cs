using System;

namespace JFS.Clients.Constants
{
    public static class EnvNames
    {
        public const string TFS_ACCESS_TOKEN = "JFS_AZURE_DEVOPS_ACCESS_TOKEN";
        public const string TFS_URI = "JFS_AZURE_DEVOPS_URI";
        public const string USE_PROXY = "JFS_USE_PROXY";

        public const string JIRA_SERVER_URI = "JFS_JIRA_SERVER_URI";
        public const string JIRA_USER = "JFS_JIRA_USER";
        public const string JIRA_PASSWORD = "JFS_JIRA_PASSWORD";
    }

    public static class Env
    {
        public static string TFS_ACCESS_TOKEN;
        public static string TFS_URI;
        public static bool USE_PROXY;

        public static string JIRA_SERVER_URI;
        public static string JIRA_USER;
        public static string JIRA_PASSWORD;

        public static void InitEnvs()
        {
            string useDefaultProxy;

            TFS_URI = Environment.GetEnvironmentVariable(EnvNames.TFS_URI);
            TFS_ACCESS_TOKEN = Environment.GetEnvironmentVariable(EnvNames.TFS_ACCESS_TOKEN);

            JIRA_SERVER_URI = Environment.GetEnvironmentVariable(EnvNames.JIRA_SERVER_URI);
            JIRA_USER = Environment.GetEnvironmentVariable(EnvNames.JIRA_USER);
            JIRA_PASSWORD = Environment.GetEnvironmentVariable(EnvNames.JIRA_PASSWORD);

            AssertEnvs();

            useDefaultProxy = Environment.GetEnvironmentVariable(EnvNames.USE_PROXY);
            USE_PROXY = (useDefaultProxy == "1" || useDefaultProxy == "True");
        }

        private static void AssertEnvs()
        {
            if (string.IsNullOrEmpty(TFS_ACCESS_TOKEN.Trim()) || 
                string.IsNullOrEmpty(TFS_URI.Trim()) ||
                string.IsNullOrEmpty(JIRA_SERVER_URI.Trim()) ||
                string.IsNullOrEmpty(JIRA_USER.Trim()) ||
                string.IsNullOrEmpty(JIRA_PASSWORD.Trim()))
            {
                throw new ApplicationException(
                    "One of required environment variables has no value.\n" +
                    "List of required variables:\n" +
                    "   JFS_AZURE_DEVOPS_ACCESS_TOKEN: Azure DevOps or TFS personal access token. Learn how to acquire it https://docs.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate?view=vsts \n" +
                    "   JFS_AZURE_DEVOPS_URI: URI of your Azure DevOps or TFS organization page e.g. 'https://dev.azure.com/{org_name}/'\n" +
                    "\n" +
                    "   JFS_JIRA_SERVER_URI: URI of your Jira Software server e.g. 'https://{org_name}.atlassian.net'\n" +
                    "   JFS_JIRA_USER: Email of your Jira Software account\n" +
                    "   JFS_JIRA_PASSWORD: Password of your Jira Software account\n" +
                    "\n" +
                    "List of optional variables:\n" +
                    "   JFS_USE_PROXY: if it is true, server will use default system proxy settings. In most cases you don't need this.");
            }
        }
    }
}
