using JetBrains.Application.DataContext;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.Application.UI.ActionSystem.ActionsRevised.Menu;
using JetBrains.ReSharper.Feature.Services.Actions;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;
using JetBrains.ReSharper.UnitTestFramework.Resources;
using ReSharperPlugin.TestingAssistant.Navigation;

namespace ReSharperPlugin.TestingAssistant.Actions
{
    [Action(Id, Name, Id = 17099, IdeaShortcuts = new[] { "Control+G Control+T" },
        VsShortcuts = new[] { "Control+G Control+T" })]
    public class GotoTestAction : ContextNavigationActionBase<LinkedTypesNavigationProvider>
    {
        public const string Id = "TestingAssistant.GotoTest";

        public const string Name = "Go to test";

        public override IActionRequirement GetRequirement(IDataContext dataContext)
        {
            return CurrentPsiFileRequirementNoCaches.FromDataContext(dataContext);
        }
    }
}