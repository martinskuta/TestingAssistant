using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Util;
using ReSharperPlugin.Settings.TestingAssistant;
using ReSharperPlugin.TestingAssistant.Mapping;
using ReSharperPlugin.TestingAssistant.Model;

namespace ReSharperPlugin.TestingAssistant.Extensions
{
    public static class ProjectExtensions
    {
        public static bool IsTestProject(this IProject project)
        {
            var currentProjectNamespace = project.GetDefaultNamespace();
            if (string.IsNullOrEmpty(currentProjectNamespace)) return false;

            var settings = SettingsManager.Instance.GetSettings(project.GetSolution());
            var regexMatcher = new Regex(settings.TestProjectToCodeProjectNameSpaceRegEx);
            return regexMatcher.IsMatch(currentProjectNamespace);      
        }

        public static IEnumerable<ProjectItem> GetAssociatedProjects(this IProject project, IProjectFile projectFile, string classNameBeingRenamed)
        {
            return MultipleTestProjectsForOneProjectMapper.GetAssociatedTestProjects(project, projectFile, classNameBeingRenamed);
        }
    }
}