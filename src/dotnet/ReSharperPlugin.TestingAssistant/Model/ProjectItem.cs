using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.Util;
using ReSharperPlugin.TestingAssistant.Extensions;
using ReSharperPlugin.TestingAssistant.Utils;

namespace ReSharperPlugin.TestingAssistant.Model
{
    public class ProjectItem
    {
        private readonly IList<Tuple<string, bool>> _subDirectoryElements;

        public ProjectItem(IProject project, ProjectItemType projectItemType, string subNameSpace,
            IList<Tuple<string, bool>> subDirectoryElements, IEnumerable<RegexFileMatcher> filePatterns)
        {
            FilePattern = filePatterns.ToArray();
            Project = project;
            SubNamespace = subNameSpace.RemoveLeading(".");

            _subDirectoryElements = subDirectoryElements;
            ProjectItemType = projectItemType;
        }

        /// <summary>
        ///     Regex Patterns for files at this location
        /// </summary>
        public RegexFileMatcher[] FilePattern { get; set; }

        /// <summary>
        ///     location should contain these file types
        /// </summary>
        public ProjectItemType ProjectItemType { get; }

        /// <summary>
        ///     Parent Project
        /// </summary>
        public IProject Project { get; }

        /// <summary>
        ///     Sub namespace of the parent project default namespace
        /// </summary>
        private string SubNamespace { get; }

        /// <summary>
        ///     Filesystem folder expected for this item (if it exists)
        /// </summary>
        public FileSystemPath SubNamespaceFolder
        {
            get
            {
                return FileSystemPath.Parse(Project.ProjectFileLocation.Directory + "\\" +
                                            _subDirectoryElements.Select(i => i.Item1).Join(@"\"));
            }
        }


        /// <summary>
        ///     Full namespace expected for this item (if it exists)
        /// </summary>
        public string FullNamespace()
        {
            if (string.IsNullOrEmpty(SubNamespace)) return Project.GetDefaultNamespace();
            return Project.GetDefaultNamespace().AppendIfMissing(".") + SubNamespace;
        }
    }
}