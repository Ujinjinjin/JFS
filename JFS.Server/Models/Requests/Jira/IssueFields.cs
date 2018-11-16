using JFS.Models.Requests.Jira;

namespace JFS.Models.Jira
{
    public class IssueFields
    {
        public string Summary { get; set; }
        public string Created { get; set; }
        public string Description { get; set; }
        public JPriority Priority { get; set; }
    }
}
