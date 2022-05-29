using JetBrains.Application.Settings;
using JetBrains.ReSharper.Resources.Settings;

namespace ReSharperPlugin.Settings.TestingAssistant
{
    [SettingsKey(typeof (CodeInspectionSettings), "Testing assistant settings")]
    public class TestingAssistantSettings
    {
        [SettingsEntry(@"^(.*?)\.?Tests?.\w*", "Regex to identify test projects by their default namespace.")]
        public string TestProjectToCodeProjectNameSpaceRegEx { get; set; }
        
        [SettingsEntry(@"", "Optional RegEx replacement pattern when renaming.")]
        public string TestProjectToCodeProjectNameSpaceRegExReplace { get; set; }
        
        [SettingsEntry("Tests", "Suffixes used to identify Test classes. Comma separated. Eg. if you have Class1 and associated test class is called Class1Tests use 'Tests' as suffix.")]
        public string TestClassSuffixes { get; set; }
    }
}