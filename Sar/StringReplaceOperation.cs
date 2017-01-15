using System.Text.RegularExpressions;

namespace Sar
{
    public class StringReplaceOperation
    {
        public string Pattern { get; set; }

        public string New { get; set; }

        public bool RegexEnabled { get; set; }

        public string GetNewString(string current)
        {
            if (!RegexEnabled)
            {
                if (current.Contains(Pattern))
                {
                    return current.Replace(Pattern, New);
                }
            }
            else
            {
                return Regex.Replace(current, Pattern, New);
            }

            return current;
        }
    }
}