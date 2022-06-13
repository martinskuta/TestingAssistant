using System.Collections.Generic;
using JetBrains.ReSharper.Psi;

namespace ReSharperPlugin.TestingAssistant.Extensions;

public static class PsiExtensions
{
    public static IEnumerable<ITypeElement> GetAllContainingTypes(this ITypeElement typeElement)
    {
        var containingType = typeElement.GetContainingType();

        while (containingType != null)
        {
            yield return containingType;
            containingType = containingType.GetContainingType();
        }
    }
}