using JetBrains.Application;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Resources.Shell;

namespace ReSharperPlugin.Settings.TestingAssistant
{
    [ShellComponent]
    public class SettingsManager
    {
        private readonly ISettingsStore _settingsStore;

        public SettingsManager(ISettingsStore settingsStore)
        {
            _settingsStore = settingsStore;
        }

        public static SettingsManager Instance => Shell.Instance.GetComponent<SettingsManager>();

        public TestingAssistantSettings GetSettings(ISolution solution)
        {
            var settingsOptimization = solution.GetComponent<ISettingsOptimization>();
            var dataContext = solution.ToDataContext();
            var context = _settingsStore.BindToContextTransient(ContextRange.Smart(dataContext));
            return context.GetKey<TestingAssistantSettings>(settingsOptimization);
        }
    }
}