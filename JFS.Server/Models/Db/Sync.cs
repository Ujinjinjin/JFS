namespace JFS.Models.Db
{
    public class Sync
    {
        public int Id { get; set; }
        public string JiraKey { get; set; }
        public int TfsId { get; set; }
        public int Rev { get; set; }
        public bool Deleted { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
