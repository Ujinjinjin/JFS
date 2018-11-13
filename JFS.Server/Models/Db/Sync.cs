namespace JFS.Models.Db
{
    public class Sync
    {
        public int Id { get; set; }
        public int JiraId { get; set; }
        public int TfsId { get; set; }
        public int Rev { get; set; }
    }
}
