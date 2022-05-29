using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.Properties;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.Util;
using ReSharperPlugin.Settings.TestingAssistant;
using ReSharperPlugin.TestingAssistant.Extensions;
using ReSharperPlugin.TestingAssistant.Model;
using ReSharperPlugin.TestingAssistant.Utils;

namespace ReSharperPlugin.TestingAssistant.Mapping
{
    public static class MultipleTestProjectsForOneProjectMapper
    {
        public static IReadOnlyList<ProjectItem> GetAssociatedTestProjects(IProject project, IProjectFile projectFile,
            string classNameBeingRenamed)
        {
            var primaryPsiFile = projectFile.GetPrimaryPsiFile();
            if (primaryPsiFile == null) return Array.Empty<ProjectItem>();

            var currentNamespace = projectFile.CalculateExpectedNamespace(primaryPsiFile.Language);

            var fileNameToProcess = projectFile.Location.NameWithoutExtension;
            fileNameToProcess = fileNameToProcess.RemoveTrailing(".partial");

            var directoryPath = ExtractFolders(projectFile);

            return GetAssociatedProject(project,
                string.IsNullOrEmpty(classNameBeingRenamed) ? fileNameToProcess : classNameBeingRenamed,
                currentNamespace, directoryPath);
        }

        private static IReadOnlyList<ProjectItem> GetAssociatedProject(IProject currentProject, string className,
            string currentTypeNamespace, IList<Tuple<string, bool>> subDirectoryElements)
        {
            var subNameSpace = currentTypeNamespace.RemoveLeading(currentProject.GetDefaultNamespace());
            var settings = SettingsManager.Instance.GetSettings(currentProject.GetSolution());

            var filePatterns = AssociatedFileNames(settings, className);

            if (currentProject.IsTestProject())
            {
                var nameSpaceOfAssociateProject = GetNameSpaceOfAssociatedCodeProject(currentProject);

                var matchedCodeProjects = currentProject.GetSolution().GetNonTestProjects().Where(
                    p => p.GetDefaultNamespace() == nameSpaceOfAssociateProject).ToList();

                return matchedCodeProjects.Select(p =>
                        new ProjectItem(p, ProjectItemType.Code, subNameSpace, subDirectoryElements, filePatterns))
                    .ToList();
            }

            var testProjects = currentProject.GetSolution().GetTestProjects();
            var matchedTestProjects = testProjects.Where(
                p => GetNameSpaceOfAssociatedCodeProject(p) == currentProject.GetDefaultNamespace()).ToList();

            return matchedTestProjects.Select(p =>
                new ProjectItem(p, ProjectItemType.Tests, subNameSpace, subDirectoryElements, filePatterns)).ToList();
        }

        private static string GetNameSpaceOfAssociatedCodeProject(IProject testProject)
        {
            var settings = SettingsManager.Instance.GetSettings(testProject.GetSolution());

            var testNameSpacePattern = settings.TestProjectToCodeProjectNameSpaceRegEx;
            var replaceText = settings.TestProjectToCodeProjectNameSpaceRegExReplace;

            var currentProjectNamespace = testProject.GetDefaultNamespace();
            if (string.IsNullOrEmpty(currentProjectNamespace)) return "";

            if (RegexReplace(testNameSpacePattern, replaceText, currentProjectNamespace, out var result)) return result;

            throw new ApplicationException(
                "Unexpected internal error. Regex failed - {0} - {1}".FormatEx(testNameSpacePattern, replaceText));
        }

        private static IEnumerable<RegexFileMatcher> AssociatedFileNames(TestingAssistantSettings settings,
            string className)
        {
            var classNameUnderTest = className;

            foreach (var suffix in settings.TestClassSuffixes())
                if (className.EndsWith(suffix))
                {
                    classNameUnderTest = className.Split(new[] { '.' }, 2)[0].RemoveTrailing(suffix);
                    break;
                }

            if (className != classNameUnderTest)
                yield return new RegexFileMatcher(new Regex(classNameUnderTest), "");
            else
                foreach (var suffix in settings.TestClassSuffixes())
                {
                    yield return
                        new RegexFileMatcher(new Regex($@"{classNameUnderTest}{suffix}"), suffix); //e.g. Class1Tests
                    yield return
                        new RegexFileMatcher(new Regex($@"{classNameUnderTest}\..*{suffix}"),
                            suffix); //e.g. Class1.SecurityTests                  
                }
        }

        private static IList<Tuple<string, bool>> ExtractFolders(IProjectItem item)
        {
            return ExtractFolders(item.ParentFolder);
        }

        private static IList<Tuple<string, bool>> ExtractFolders(IProjectFolder currentFolder)
        {
            IList<Tuple<string, bool>> foldersList = new List<Tuple<string, bool>>();

            var namespaceFolderProperty = currentFolder.GetSolution().GetComponent<NamespaceFolderProperty>();

            while (currentFolder != null)
            {
                if (currentFolder.Kind == ProjectItemKind.PHYSICAL_DIRECTORY)
                    foldersList.Insert(0,
                        new Tuple<string, bool>(currentFolder.Name,
                            namespaceFolderProperty.GetNamespaceFolderProperty(currentFolder)));
                currentFolder = currentFolder.ParentFolder;
            }

            return foldersList;
        }

        private static bool RegexReplace(string regexPattern, string regexReplaceText, string inputString,
            out string resultString)
        {
            resultString = "";
            var regex = new Regex(regexPattern);
            var match = regex.Match(inputString);

            if (match.Success && match.Groups.Count > 1)
            {
                if (regexReplaceText.IsNullOrEmpty() || regexReplaceText == "*")
                {
                    for (var i = 1; i < match.Groups.Count; i++) resultString += match.Groups[i].Value;
                    return true;
                }

                resultString = regex.Replace(inputString, regexReplaceText);
                return true;
            }

            return false;
        }
    }
}