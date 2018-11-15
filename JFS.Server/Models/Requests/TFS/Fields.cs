using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JFS.Models.Requests.TFS
{
    public class Fields
    {
        [JsonProperty(PropertyName = "System.WorkItemType")]
        public string WorkItemType { get; set; }

        [JsonProperty(PropertyName = "System.State")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "System.CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "System.Title")]
        public string Title { get; set; }
    }
}
