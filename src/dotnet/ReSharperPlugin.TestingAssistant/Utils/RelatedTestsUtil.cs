using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.Util;
using ReSharperPlugin.Settings.TestingAssistant;
using ReSharperPlugin.TestingAssistant.Extensions;

namespace ReSharperPlugin.TestingAssistant.Utils
{
    public static class RelatedTestsUtil
    {
        public static IReadOnlyCollection<ITypeElement> GetRelatedTypesIncludingSuperTypes(ITypeElement source)
        {
            var baseTypesExceptObjectClass = source.GetAllSuperTypes()
                .Select(x => x.GetTypeElement())
                .WhereNotNull()
                .Where(x => !x.IsObjectClass());
            
            var sourceTypesPlusContainingTypes = new[] { source }
                .Concat(baseTypesExceptObjectClass)
                .SelectMany(x => x.GetAllContainingTypes().Concat(x)).ToList();

            var linkedTypes = sourceTypesPlusContainingTypes.SelectMany(GetLinkedTypesInternal).ToList();

            return linkedTypes;
        }
        
        public static IReadOnlyCollection<ITypeElement> GetRelatedTypes(ITypeElement source) => GetLinkedTypesInternal(source);

        private static IReadOnlyCollection<ITypeElement> GetLinkedTypesInternal(ITypeElement source)
        {
            if (!(source.Module is IProjectPsiModule psiModule)) return Array.Empty<ITypeElement>();

            var derivedNames = psiModule.Project.IsTestProject()
                ? GetDerivedTestedClassNames(source)
                : GetDerivedTestClassNames(source);

            var psiServices = source.GetPsiServices();

            var symbolCache = psiServices.Symbols.GetSymbolScope(LibrarySymbolScope.NONE, true);

            return derivedNames
                .SelectMany(x => symbolCache.GetElementsByShortName(x))
                .OfType<ITypeElement>()
                .Where(x => !x.Equals(source))
                .ToList();
        }

        private static IReadOnlyCollection<string> GetDerivedTestClassNames(IDeclaredElement source)
        {
            var settings = SettingsManager.Instance.GetSettings(source.GetSolution());
            var testClassSuffixes = settings.TestClassSuffixes();
            
            var shortName = source.ShortName;
            var names = new List<string>();

            foreach (var suffix in testClassSuffixes)
            {
                names.Add(shortName + suffix);
                names.Add(suffix + shortName);
            }

            return names;
        }

        private static IReadOnlyCollection<string> GetDerivedTestedClassNames(IDeclaredElement source)
        {
            var settings = SettingsManager.Instance.GetSettings(source.GetSolution());
            var testClassSuffixes = settings.TestClassSuffixes();
            
            var shortName = source.ShortName;
            var names = new HashSet<string>();

            foreach (var suffix in testClassSuffixes)
            {
                var trimEnd = shortName.TrimFromEnd(suffix);
                var trimStart = shortName.TrimFromStart(suffix);

                if (trimEnd != shortName) names.Add(trimEnd);
                if (trimStart != shortName) names.Add(trimStart);
            }

            return names;
        }
    }
}