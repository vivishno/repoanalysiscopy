namespace GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using GitHub.RepositoryAnalysis.Detectors.Utilities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class DotNetFrameworkWebBuildTargetDetector: DotNetFrameworkBuildTargetDetectorBase
    {
        public override string BuildTargetName => Constants.DotNetFrameworkWebBuildTarget;

        public override bool IsBuildTargetDetected(TreeAnalysis treeAnalysis)
        {
            if (treeAnalysis.FilesInfo.ContainsKey(Constants.WebConfigFileName))
            {
                IList<FileSystemTreeNode> webConfigFileList = treeAnalysis.FilesInfo[Constants.WebConfigFileName];
                var projectFileNames = new List<string>(treeAnalysis.FilesContent.Keys.Where(x => Regex.IsMatch(x, Constants.CsProjDetectionRegex, RegexOptions.IgnoreCase)));
                List<CsprojMetadata> eligibleProjects = (from wcFile in webConfigFileList
                                                         join projectFileName in projectFileNames
                                                         on wcFile.GetDirectoryPath() equals GetDirectoryPathFromString(projectFileName)
                                                         select new CsprojMetadata
                                                         {
                                                             ProjectName = projectFileName,
                                                             TargetFramework = CsprojParser.GetNetFxVersion(treeAnalysis.FilesContent[projectFileName].ToXMLDoc())
                                                         }).ToList();
                if (eligibleProjects.Count > 0)
                {
                    QualifiedProjects.AddRange(eligibleProjects);
                    return true;
                }
            }

            return false;
        }
    }
}
