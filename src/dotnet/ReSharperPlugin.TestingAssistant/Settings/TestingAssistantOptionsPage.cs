using System;
using System.Linq.Expressions;
using JetBrains.Application.Settings;
using JetBrains.Application.UI.Icons.CommonThemedIcons;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionsDialog;
using JetBrains.DataFlow;
using JetBrains.IDE.UI.Extensions;
using JetBrains.IDE.UI.Options;
using JetBrains.Lifetimes;
using JetBrains.ReSharper.UnitTestFramework.UI.Options;

namespace ReSharperPlugin.Settings.TestingAssistant
{
    [OptionsPage(PageId, PageTitle, typeof(CommonThemedIcons.Bulb),
        ParentId = UnitTestingPages.General,
        NestingType = OptionPageNestingType.Inline,
        IsAlignedWithParent = true,
        Sequence = 0.1d)]
    public class TestingAssistantOptionsPage: BeSimpleOptionsPage
    {
        private const string PageId = nameof(TestingAssistantOptionsPage);
        private const string PageTitle = "Testing assistant";

        private readonly Lifetime _lifetime;

        public TestingAssistantOptionsPage(
            Lifetime lifetime,
            OptionsPageContext optionsPageContext,
            OptionsSettingsSmartContext optionsSettingsSmartContext)
            : base(lifetime, optionsPageContext, optionsSettingsSmartContext)
        {
            _lifetime = lifetime;
            
            AddHeader("Navigation");

            AddTextBox((TestingAssistantSettings x) => x.TestProjectToCodeProjectNameSpaceRegEx, "Regex to identify test projects by their default namespace.");
            AddTextBox((TestingAssistantSettings x) => x.TestClassSuffixes, "Suffixes used to identify Test classes. Comma separated.");
            
            AddHeader("Rename");
            AddTextBox((TestingAssistantSettings x) => x.TestProjectToCodeProjectNameSpaceRegExReplace, "Optional RegEx replacement pattern when renaming.");
        }

        private void AddTextBox<TKeyClass>(Expression<Func<TKeyClass, string>> lambdaExpression, string description)
        {
            var property = new Property<string>(description);
            OptionsSettingsSmartContext.SetBinding(_lifetime, lambdaExpression, property);
            var control = property.GetBeTextBox(_lifetime);
            AddControl(control.WithDescription(description, _lifetime));
        }
    }
}