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
                case "Minor":
                    return 1;
                case "Major":
                    return 2;
                default:
                    return 1;
            }
        }
    }
}
