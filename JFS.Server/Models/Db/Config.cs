namespace JFS.Models.Db
{
    public class Config
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public string Sprint { get; set; }
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
        public int WorkItemTypeId { get; set; }
        public WorkItemType WorkItemType { get; set; }
    }
}
