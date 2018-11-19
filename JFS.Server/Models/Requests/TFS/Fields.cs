using Newtonsoft.Json;

namespace JFS.Models.Requests.TFS
{
    public class Fields<T>
    {
        [JsonProperty(PropertyName = "System.WorkItemType")]
        public T WorkItemType { get; set; }

        [JsonProperty(PropertyName = "System.State")]
        public T State { get; set; }

        [JsonProperty(PropertyName = "System.CreatedBy")]
        public T CreatedBy { get; set; }

        [JsonProperty(PropertyName = "System.Title")]
        public T Title { get; set; }

        [JsonProperty(PropertyName = "Microsoft.VSTS.TCM.ReproSteps")]
        public T ReproSteps { get; set; }

        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.Priority")]
        public T Priority { get; set; }
    }
}
