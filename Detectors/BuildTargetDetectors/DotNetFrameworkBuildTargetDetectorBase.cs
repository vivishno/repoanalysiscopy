namespace GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using GitHub.RepositoryAnalysis.Detectors.Utilities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml;

    public abstract class DotNetFrameworkBuildTargetDetectorBase : BuildTargetDetectorBase
    {
        public List<CsprojMetadata> QualifiedProjects;
        public DotNetFrameworkBuildTargetDetectorBase()
        {
            QualifiedProjects = new List<CsprojMetadata>();
        }
        public override List<BuildTargetSettings> GetBuildTargetSettings(TreeAnalysis treeAnalysis)
        {
            List<BuildTargetSettings> buildTargetSettingsList = new List<BuildTargetSettings>();

            foreach (var qualifiedProject in QualifiedProjects)
            {
                var node = treeAnalysis.FilesInfo[qualifiedProject.ProjectName.Split('/').LastOrDefault()].First();
                
                buildTargetSettingsList.Add(new BuildTargetSettings
                {
                    Name = BuildTargetName,
                    Settings = new Dictionary<string, object>
                    {
                        { Constants.WorkingDirectory, node.GetDirectoryPath()},
                        { Constants.LanguageVersion, qualifiedProject.TargetFramework},
                        { Constants.SolutionDirectory, GetSolutionPath(treeAnalysis, node)},
                        { Constants.CsprojPath, node.Path }
                    }
                });
            }

            return buildTargetSettingsList;
        }

        private string GetSolutionPath(TreeAnalysis treeAnalysis, FileSystemTreeNode csprojNode)
        {
            // TODO: Pass requestContext to all the detectors and add trace
            var solutionFiles = treeAnalysis.FilesInfo.Keys.Where(x => x.EndsWith(Constants.SolutionFileExtension));
            foreach(var solutionFile in solutionFiles)
            {
                var eligibleSolutionNodes = treeAnalysis.FilesInfo[solutionFile].Where(node => node.IsAncestorOf(csprojNode)).Select(node => node.GetDirectoryPath());
                if (eligibleSolutionNodes.Count() > 0)
                {
                    return eligibleSolutionNodes.FirstOrDefault();
                }
            }

            return csprojNode.GetDirectoryPath();
        }

        public override bool IsBuildTargetDetected(TreeAnalysis treeAnalysis)
        {
            var filesReadNames = new List<string>(treeAnalysis.FilesContent.Keys.Where(x => Regex.IsMatch(x, Constants.CsProjDetectionRegex, RegexOptions.IgnoreCase)));
            foreach (var csprojFileName in filesReadNames)
            {
                var curProjMeta = GetMetadataIfBuildTargetMatched(treeAnalysis, csprojFileName);
                if (curProjMeta != null)
                    QualifiedProjects.Add(curProjMeta);
            }
            if (QualifiedProjects.Count > 0) return true;
            return false;
        }

        public virtual CsprojMetadata GetMetadataIfBuildTargetMatched(TreeAnalysis treeAnalysis, string csprojFileName)
        {
            var currentCsproj = treeAnalysis.FilesContent[csprojFileName].ToXMLDoc();
            if (currentCsproj == null)
                return null;
            if (IsBuildTargetMatched(currentCsproj))
            {
                var targetFramework = CsprojParser.GetNetFxVersion(currentCsproj);
                return new CsprojMetadata
                {
                    ProjectName = csprojFileName.Split('/').LastOrDefault(),
                    TargetFramework = targetFramework
                };
            }
            return null;
        }

        public virtual bool IsBuildTargetMatched(XmlDocument currentCsproj)
        {
            return !string.IsNullOrEmpty(CsprojParser.GetNetFxVersion(currentCsproj)) || Constants.DotNetFramework.Equals(CsprojParser.GetDotNetVersion(currentCsproj).Key);
        }
    }
}
