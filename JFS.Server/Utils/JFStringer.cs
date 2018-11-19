namespace JFS.Utils
{
    public static class JFStringer
    {
        public static string ToCommonFormat(string oldString)
        {
            return oldString != null ? oldString.Replace("</div><div>", "\n").Replace("</div>", "").Replace("<div>", "").Replace("<br>", "") : "";
        }

        public static string ToTfsFormat(string oldsString)
        {
            return oldsString != null ? $"<div>{oldsString.Replace("\n", "</div><div>")}</div>" : "";
        }
    }
}
