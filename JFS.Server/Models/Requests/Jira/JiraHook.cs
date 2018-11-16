namespace JFS.Models.Jira
{
    public class JiraHook
    {
        public Issue Issue { get; set; }
        public User User { get; set; }
        public ChangeLog ChangeLog { get; set; }
        public string WebhookEvent { get; set; }
    }
}
