namespace JFS.Models.Db
{
    public class TfsConfig
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public int ParentId { get; set; }
        public string Iteration { get; set; }
        public string Area { get; set; }
        public string TeamProject { get; set; }
    }
}
