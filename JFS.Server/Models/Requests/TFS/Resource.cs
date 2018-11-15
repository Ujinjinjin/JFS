using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JFS.Models.Requests.TFS
{
    public class Resource
    {
        public int Id { get; set; }
        public int Rev { get; set; }
        public Fields<string> Fields { get; set; }
    }
}
