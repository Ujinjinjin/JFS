using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JFS.Utils
{
    public static class JFStringer
    {
        public static string ToCommonFormat(string oldsString)
        {
            return oldsString.Replace("</div><div>", "\n").Replace("</div>", "").Replace("<div>", "").Replace("<br>", "");
        }

        public static string ToTfsFormat(string oldsString)
        {
            return $"<div>{oldsString.Replace("\n", "</div><div>")}</div>";
        }
    }
}
