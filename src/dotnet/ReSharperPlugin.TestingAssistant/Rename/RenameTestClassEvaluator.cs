using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Refactorings.Rename;
using ReSharperPlugin.TestingAssistant.Utils;

namespace ReSharperPlugin.TestingAssistant.Rename;

[DerivedRenamesEvaluator]
public class RenameTestClassEvaluator : IDerivedRenamesEvaluator
{
    public IEnumerable<IDeclaredElement> CreateFromElement(IEnumerable<IDeclaredElement> initialElement,
        DerivedElement derivedElement)
    {
        var derivedElements = new List<IDeclaredElement>();

        foreach (var declaredElement in initialElement)
        {
            if (declaredElement is not ITypeElement typeElement)
                continue;

            derivedElements.AddRange(RelatedTestsUtil.GetRelatedTypes(typeElement));
        }

        return derivedElements;
    }

    public IEnumerable<IDeclaredElement> CreateFromReference(IReference reference, IDeclaredElement declaredElement)
    {
        return Array.Empty<IDeclaredElement>();
    }

    public bool SuggestedElementsHaveDerivedName => true;
}