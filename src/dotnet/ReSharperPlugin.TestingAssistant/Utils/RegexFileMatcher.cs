using System.Text.RegularExpressions;

namespace ReSharperPlugin.TestingAssistant.Utils
{
    public class RegexFileMatcher
    {
        public RegexFileMatcher(Regex regEx, string suffix)
        {
            Suffix = suffix;
            RegEx = regEx;
        }

        public string Suffix { get; }
        public Regex RegEx { get; }
    }
}