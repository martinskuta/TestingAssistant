using System.Collections.Generic;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Refactorings.Specific.Rename;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.Refactorings.Rename;
using JetBrains.Util;
using ReSharperPlugin.TestingAssistant.Extensions;
using ReSharperPlugin.TestingAssistant.Model;

namespace ReSharperPlugin.TestingAssistant.Rename
{
    [FileRenameProvider]
    public class RelatedFileRenameProvider : IFileRenameProvider
    {
        public IEnumerable<FileRename> GetFileRenames(
            IDeclaredElement declaredElement,
            string name)
        {
            if (!(declaredElement is IClrDeclaredElement clrDeclaredElement)) yield break;
            if (!(declaredElement is ITypeElement)) yield break;
            if (!(clrDeclaredElement.Module is IProjectPsiModule psiModule)) yield break;

            var declaredElementProject = psiModule.Project;
            if (declaredElementProject.IsTestProject()) yield break;

            var classNameBeingRenamed = declaredElement.ShortName;
            var solution = declaredElementProject.GetSolution();

            var targetTestProjects = new List<ProjectItem>();
            foreach (var sourceFile in declaredElement.GetSourceFiles())
            {
                var projectFile = sourceFile.ToProjectFile();
                targetTestProjects.AddRange(
                    declaredElementProject.GetAssociatedProjects(projectFile, classNameBeingRenamed));
            }

            if (targetTestProjects.IsNullOrEmpty()) yield break;

            foreach (var targetProject in targetTestProjects)
            {
                var projectTestFilesWithMatchingName = new List<ProjectFileFinder.Match>();
                targetProject.Project.Accept(new ProjectFileFinder(projectTestFilesWithMatchingName,
                    targetProject.FilePattern));

                foreach (var projectFileMatch in projectTestFilesWithMatchingName)
                {
                    var primaryPsiFile = projectFileMatch.ProjectFile.GetPrimaryPsiFile();
                    if (primaryPsiFile == null) continue;

                    var expectedNameSpace =
                        projectFileMatch.ProjectFile.CalculateExpectedNamespace(primaryPsiFile.Language);

                    if (expectedNameSpace != targetProject.FullNamespace()) continue;

                    var currentName = projectFileMatch.ProjectFile.Location.NameWithoutExtension;
                    var newTestClassName = name + currentName.Substring(classNameBeingRenamed.Length);

                    solution.GetComponent<IEditorManager>()
                        .OpenProjectFileAsync(projectFileMatch.ProjectFile, new OpenFileOptions(false));

                    yield return new FileRename(psiModule.GetPsiServices(), projectFileMatch.ProjectFile,
                        newTestClassName);
                }
            }
        }
    }
}