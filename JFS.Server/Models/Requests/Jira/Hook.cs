namespace JFS.Models.Jira
{
    public class JiraHook
    {
        public int Id { get; set; }
        public Issue Issue { get; set; }
        public User User { get; set; }
        public ChangeLog ChangeLog { get; set; }
        public string WebhookEvent { get; set; }
    }
}
