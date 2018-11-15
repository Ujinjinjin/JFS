using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JFS
{
    public static class Mapper
    {
        public static int JiraPriorityToTfs(string priority)
        {
            switch (priority)
            {
                case "Major":
                    return 1;
                case "Minor":
                    return 2;
                default:
                    return 1;
            }
        }

        public static string TfsPriorityToJira(int priority)
        {
            switch (priority)
            {
                case 1:
                    return "Major";
                case 2:
                    return "Minor";
                default:
                    return "Major";
            }
        }
    }
}
