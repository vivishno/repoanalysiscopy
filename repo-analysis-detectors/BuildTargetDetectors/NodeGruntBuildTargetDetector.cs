namespace GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using System.Collections.Generic;

    public class NodeGruntBuildTargetDetector : BuildTargetDetectorBase
    {
        public override string BuildTargetName => Constants.GruntTaskRunner;

        public override List<BuildTargetSettings> GetBuildTargetSettings(TreeAnalysis treeAnalysis)
        {
            /*Logic: 
             * Working Directory:
             * Presence of Package.json along with gruntfile.js file in same directory defines the working directory.
            */

            List<BuildTargetSettings> buildTargetSettingsList = new List<BuildTargetSettings>();
            IList<FileSystemTreeNode> packageJsonFiles = treeAnalysis.FilesInfo[Constants.PackageJsonFileName];
            IList<FileSystemTreeNode> gruntFiles = treeAnalysis.FilesInfo[Constants.GruntFileName];

            foreach (FileSystemTreeNode packageJsonFile in packageJsonFiles)
            {
                foreach (FileSystemTreeNode gruntFile in gruntFiles)
                {
                    if (packageJsonFile.IsAncestorOf(gruntFile))
                    {
                        BuildTargetSettings buildTargetSetting = new BuildTargetSettings()
                        {
                            Name = BuildTargetName,
                            Settings = new Dictionary<string, object>()
                            {
                                { Constants.WorkingDirectory, packageJsonFile.GetDirectoryPath() },
                                { Constants.PackageJsonFilePath, packageJsonFile.Path },
                                { Constants.GruntFilePath, gruntFile.Path}
                            }
                        };
                        buildTargetSettingsList.Add(buildTargetSetting);
                    }
                }
            }
            return buildTargetSettingsList;
        }

        public override bool IsBuildTargetDetected(TreeAnalysis treeAnalysis)
        {
            if (treeAnalysis.FilesInfo.ContainsKey(Constants.GruntFileName))
            {
                return true;
            }
            return false;
        }
    }
}
