using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JFS.Models.Requests.TFS
{
    public class TfsHook
    {
        public string EventType { get; set; }
        public Resource Resource { get; set; }
    }
}
