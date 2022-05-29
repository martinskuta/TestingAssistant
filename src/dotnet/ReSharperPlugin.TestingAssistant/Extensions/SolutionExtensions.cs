using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;

namespace ReSharperPlugin.TestingAssistant.Extensions
{
    public static class SolutionExtensions
    {
        public static IEnumerable<IProject> GetTestProjects(this ISolution solution)
        {
            return GetAllCodeProjects(solution).Where(x => x.IsTestProject());
        }

        public static IEnumerable<IProject> GetNonTestProjects(this ISolution solution)
        {
            return GetAllCodeProjects(solution).Where(x => !x.IsTestProject());
        }

        private static IEnumerable<IProject> GetAllCodeProjects(this IProjectCollection solution)
        {
            return solution.GetAllProjects().Where(p => p.IsProjectFromUserView());
        }
    }
}