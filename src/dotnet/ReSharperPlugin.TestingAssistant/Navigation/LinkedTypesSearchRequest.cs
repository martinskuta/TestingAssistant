using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Navigation.Requests;
using JetBrains.ReSharper.Feature.Services.Occurrences;
using JetBrains.ReSharper.Psi;
using JetBrains.TextControl;
using JetBrains.Util;
using ReSharperPlugin.TestingAssistant.Utils;

namespace ReSharperPlugin.TestingAssistant.Navigation
{
    public class LinkedTypesSearchRequest : SearchRequest
    {
        private readonly bool _derivedNamesOnly;
        private readonly ITextControl _textControl;
        private readonly ITypeElement _typeElement;

        public LinkedTypesSearchRequest(ITypeElement typeElement, ITextControl textControl, bool derivedNamesOnly)
        {
            _typeElement = typeElement;
            _textControl = textControl;
            _derivedNamesOnly = derivedNamesOnly;
        }

        public override string Title => $"Related files for {_typeElement.ShortName}";

        public override ISolution Solution => _typeElement.GetSolution();

        public override ICollection SearchTargets => new IDeclaredElementEnvoy[]
            { new DeclaredElementEnvoy<IDeclaredElement>(_typeElement) };

        public override ICollection<IOccurrence> Search(IProgressIndicator progressIndicator)
        {
            if (!_typeElement.IsValid())
                return EmptyList<IOccurrence>.InstanceList;

            var linkedTypes = RelatedTestsUtil.GetRelatedTypes(_typeElement);
            
            // if (linkedTypes.Count == 0) Suggest to create new tests file

            bool IsDerivedName(ITypeElement typeElement)
            {
                return _typeElement.ShortName.Contains(typeElement.ShortName) ||
                       typeElement.ShortName.Contains(_typeElement.ShortName);
            }

            return linkedTypes
                .Select(x => new LinkedTypesOccurrence(x, OccurrenceType.Occurrence, IsDerivedName(x)))
                .Where(x => !_derivedNamesOnly || x.HasNameDerived)
                .ToArray();
        }
    }
}