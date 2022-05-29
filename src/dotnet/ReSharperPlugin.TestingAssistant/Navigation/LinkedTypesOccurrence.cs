using JetBrains.Application.UI.PopupLayout;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Navigation.NavigationExtensions;
using JetBrains.ReSharper.Feature.Services.Occurrences;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;

namespace ReSharperPlugin.TestingAssistant.Navigation
{
    public class LinkedTypesOccurrence : DeclaredElementOccurrence
    {
        public LinkedTypesOccurrence (
            IDeclaredElement element,
            OccurrenceType occurrenceKind,
            bool hasNameDerived)
            : base(element, occurrenceKind)
        {
            HasNameDerived = hasNameDerived;
        }

        public bool HasNameDerived { get; }

        public override bool Navigate(
            ISolution solution,
            PopupWindowContextSource windowContext,
            bool transferFocus,
            TabOptions tabOptions = TabOptions.Default)
        {
            var textControlManager = solution.GetComponent<ITextControlManager>();

            var declaredElement = OccurrenceElement?.GetValidDeclaredElement();
            if (declaredElement == null)
                return false;

            foreach (var declaration in declaredElement.GetDeclarations())
            {
                var sourceFile = declaration.GetSourceFile();
                if (sourceFile == null)
                    continue;

                foreach (var textControl in textControlManager.TextControls)
                {
                    if (textControl.Document != sourceFile.Document)
                        continue;

                    var declarationRange = declaration.GetDocumentRange();
                    var textControlOffset = textControl.Caret.DocumentOffset();
                    if (!declarationRange.Contains(textControlOffset))
                        continue;

                    var popupWindowContextSource = solution.GetComponent<IMainWindowPopupWindowContext>().Source;
                    return sourceFile.Navigate(
                        textControl.Selection.OneDocRangeWithCaret(),
                        transferFocus,
                        tabOptions,
                        popupWindowContextSource);
                }
            }

            return base.Navigate(solution, windowContext, transferFocus, tabOptions);
        }
    }
}