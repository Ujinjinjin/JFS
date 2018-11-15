using System.Collections.Generic;

namespace JFS.Models.Jira
{
    public class ChangeLog
    {
        public int Id { get; set; }
        public List<ChangeLogItem> Items { get; set; }
    }
}
