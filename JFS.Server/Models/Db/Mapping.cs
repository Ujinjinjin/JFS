namespace JFS.Models.Db
{
    public class Mapping
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
        public int CommonFieldId { get; set; }
        public CommonField CommonField { get; set; }
        public int TfsFieldId { get; set; }
        public TfsField TfsField { get; set; }
        public int JiraFieldId { get; set; }
        public JiraField JiraField { get; set; }
    }
}
