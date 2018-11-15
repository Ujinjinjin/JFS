using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JFS.Models.Requests.TFS
{
    public class UpdatedResource
    {
        public Resource Revision { get; set; }
        public Fields<Changes> Fields { get; set; }
    }
}
