namespace JFS.Models.Jira
{
    public class Issue
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public IssueFields Fields { get; set; }
    }
}
