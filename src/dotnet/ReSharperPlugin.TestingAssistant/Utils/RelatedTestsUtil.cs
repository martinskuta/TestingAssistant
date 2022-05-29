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
        public static IReadOnlyCollection<ITypeElement> GetRelatedTypes(ITypeElement source)
        {
            var sources = new[] { source }
                .Concat(source.GetAllSuperTypes()
                    .Select(x => x.GetTypeElement())
                    .WhereNotNull()
                    .Where(x => !x.IsObjectClass()));

            var linkedTypes = sources.SelectMany(GetLinkedTypesInternal).ToList();

            return linkedTypes;
        }

        private static IReadOnlyCollection<ITypeElement> GetLinkedTypesInternal(ITypeElement source)
        {
            if (!(source.Module is IProjectPsiModule psiModule)) return Array.Empty<ITypeElement>();

            var settings = SettingsManager.Instance.GetSettings(source.GetSolution());
            var testClassSuffixes = settings.TestClassSuffixes();

            var derivedNames = psiModule.Project.IsTestProject()
                ? GetDerivedTestedClassNames(source, testClassSuffixes)
                : GetDerivedTestClassNames(source, testClassSuffixes);

            var psiServices = source.GetPsiServices();

            var symbolCache = psiServices.Symbols.GetSymbolScope(LibrarySymbolScope.NONE, true);

            return derivedNames
                .SelectMany(x => symbolCache.GetElementsByShortName(x))
                .OfType<ITypeElement>()
                .Where(x => !x.Equals(source))
                .ToList();
        }

        private static IReadOnlyCollection<string> GetDerivedTestClassNames(ITypeElement source,
            IReadOnlyCollection<string> suffixes)
        {
            var shortName = source.ShortName;
            var names = new List<string>();

            foreach (var suffix in suffixes)
            {
                names.Add(shortName + suffix);
                names.Add(suffix + shortName);
            }

            return names;
        }

        private static IReadOnlyCollection<string> GetDerivedTestedClassNames(ITypeElement source,
            IReadOnlyCollection<string> suffixes)
        {
            var shortName = source.ShortName;
            var names = new HashSet<string>();

            foreach (var suffix in suffixes)
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