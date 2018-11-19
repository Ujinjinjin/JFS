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

        public static int ToTfsPriority(ApplicationDbContext context, string jiraPriority)
        {
            return context.Priority.First(p => p.JiraPriority == jiraPriority).TfsPriority; ;
        }

        public static string ToJiraPriority(ApplicationDbContext context, int tfsPriority)
        {
            return context.Priority.First(p => p.TfsPriority == tfsPriority).JiraPriority;
        }

        public static string ToJiraPriority(ApplicationDbContext context, string tfsPriority)
        {
            return context.Priority.First(p => p.TfsPriority == int.Parse(tfsPriority)).JiraPriority; ;
        }
    }
}
