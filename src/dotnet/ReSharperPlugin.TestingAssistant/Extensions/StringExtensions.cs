using System.Linq;

namespace ReSharperPlugin.TestingAssistant.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveLeading(this string refString, string value)
        {
            return refString.StartsWith(value) ? refString.Substring(value.Length) : refString;
        }
        
        public static string RemoveTrailing(this string refString, string value)
        {
            return refString.EndsWith(value) ? refString.Substring(0, refString.Length - value.Length) : refString;
        }
        
        public static string AppendIfMissing(this string refString, string value)
        {
            return refString.EndsWith(value) ? refString : refString + value;
        }
    }
}