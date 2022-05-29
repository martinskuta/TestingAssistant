using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using ReSharperPlugin.TestingAssistant.Utils;

namespace ReSharperPlugin.TestingAssistant.Model
{
    public class ProjectFileFinder : RecursiveProjectVisitor
    {
        private readonly RegexFileMatcher[] _fileMatchers;        
        private readonly List<Match> _itemMatches;

        public struct Match
        {
            public Match(IProjectFile projectFile)
            {
                ProjectFile = projectFile;
            }

            public IProjectFile ProjectFile { get; }
        }


        public ProjectFileFinder(List<Match> itemMatches, params RegexFileMatcher[] fileMatchers)
        {
            _itemMatches = itemMatches;            
            _fileMatchers = fileMatchers;
        }

        public override void VisitProjectFile(IProjectFile projectFile)
        {
            base.VisitProjectFile(projectFile);
            var projectFileName = projectFile.Location.NameWithoutExtension;

            if (projectFile.Kind != ProjectItemKind.PHYSICAL_FILE) return;
            
            if (_fileMatchers.Any(pattern => pattern.RegEx.IsMatch(projectFileName)))
            {
                _itemMatches.Add(new Match(projectFile));
            }
        }
    }
}