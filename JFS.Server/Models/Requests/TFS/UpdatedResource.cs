namespace JFS.Models.Requests.TFS
{
    public class UpdatedResource
    {
        public Resource Revision { get; set; }
        public Fields<Changes> Fields { get; set; }
    }
}
