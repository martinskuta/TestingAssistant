using System.Collections.Generic;
using JetBrains.Application;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;
using JetBrains.ReSharper.Feature.Services.Navigation.ExecutionHosting;
using JetBrains.ReSharper.Feature.Services.Navigation.Requests;
using JetBrains.ReSharper.Feature.Services.Occurrences;
using JetBrains.ReSharper.Features.Navigation.Features.FindHierarchy;
using ReSharperPlugin.TestingAssistant.Actions;

namespace ReSharperPlugin.TestingAssistant.Navigation
{
    [ContextNavigationProvider]
    public class LinkedTypesNavigationProvider: HierarchyProviderBase<LinkedTypesContextSearch, LinkedTypesSearchRequest, LinkedTypesSearchDescriptor>, INavigateFromHereProvider
    {
        public LinkedTypesNavigationProvider(IFeaturePartsContainer manager)
            : base(manager)
        {
        }

        public override string GetNotFoundMessage(SearchRequest request) => "No linked types found";

        protected override OccurrencePresentationOptions ProvideFeatureSpecificPresentationOptions(LinkedTypesSearchRequest searchRequest)
        {
            return new OccurrencePresentationOptions();
        }

        protected override LinkedTypesSearchDescriptor CreateSearchDescriptor(LinkedTypesSearchRequest searchRequest, ICollection<IOccurrence> results)
        {
            return new LinkedTypesSearchDescriptor(searchRequest, results);
        }

        public IEnumerable<ContextNavigation> CreateWorkflow(IDataContext dataContext)
        {
            var solution = dataContext.GetData(ProjectModelDataConstants.SOLUTION);
            var navigationExecutionHost = DefaultNavigationExecutionHost.GetInstance(solution);

            var execution = GetSearchesExecution(dataContext, navigationExecutionHost);
            if (execution != null)
            {
                yield return new ContextNavigation(
                    GotoTestAction.Name,
                    GotoTestAction.Id,
                    NavigationActionGroup.Important,
                    execution);
            }
        }
    }
}