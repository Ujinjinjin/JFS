using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JFS.Models.Db
{
    public class Priority
    {
        public int Id { get; set; }
        public int TfsPriority { get; set; }
        public string JiraPriority { get; set; }
    }
}
