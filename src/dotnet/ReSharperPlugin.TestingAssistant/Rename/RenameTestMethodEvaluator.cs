using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Refactorings.Rename;

namespace ReSharperPlugin.TestingAssistant.Rename
{
    [DerivedRenamesEvaluator]
    public class RenameTestMethodEvaluator : IDerivedRenamesEvaluator
    {
        public IEnumerable<IDeclaredElement> CreateFromElement(IEnumerable<IDeclaredElement> initialElement, DerivedElement derivedElement) => Array.Empty<IDeclaredElement>();

        public IEnumerable<IDeclaredElement> CreateFromReference(IReference reference, IDeclaredElement declaredElement)
        {
            if (!(reference.GetTreeNode() is ICSharpTreeNode cSharpTreeNode))
                return Array.Empty<IDeclaredElement>();
            
            var containingFunction = cSharpTreeNode.GetContainingFunctionDeclarationIgnoringClosures();
            if (containingFunction == null)
                return Array.Empty<IDeclaredElement>();

            return containingFunction.DeclaredName.Contains(declaredElement.ShortName)
                ? new[] { containingFunction.DeclaredElement }
                : Array.Empty<IDeclaredElement>();
        }

        public bool SuggestedElementsHaveDerivedName => true;
    }
}