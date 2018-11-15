﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}