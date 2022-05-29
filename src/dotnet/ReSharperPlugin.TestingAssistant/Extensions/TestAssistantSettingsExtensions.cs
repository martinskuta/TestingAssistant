using System.Collections.Generic;
using System.Linq;
using ReSharperPlugin.Settings.TestingAssistant;

namespace ReSharperPlugin.TestingAssistant.Extensions
{
    public static class TestingAssistantSettingsExtensions
    {
        public static IReadOnlyList<string> TestClassSuffixes(this TestingAssistantSettings settings)
        {
            var list = (settings.TestClassSuffixes ?? "").Split(',').ToList();
            list.RemoveAll(string.IsNullOrEmpty);
            list.Sort((a, b) => b.Length - a.Length);
            return list;
        }
    }
}