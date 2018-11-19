namespace JFS.Models.Requests.TFS
{
    public class Resource
    {
        public int Id { get; set; }
        public int Rev { get; set; }
        public Fields<string> Fields { get; set; }
    }
}
