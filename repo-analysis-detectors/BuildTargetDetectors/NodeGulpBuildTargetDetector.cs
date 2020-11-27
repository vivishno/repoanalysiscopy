namespace GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using System.Collections.Generic;

    public class NodeGulpBuildTargetDetector : BuildTargetDetectorBase
    {
        public override string BuildTargetName => Constants.GulpTaskRunner;

        public override List<BuildTargetSettings> GetBuildTargetSettings(TreeAnalysis treeAnalysis)
        {
            /*Logic: 
             * Working Directory:
             * Presence of Package.json along with gulpfile.js file in same directory defines the working directory.
            */

            List<BuildTargetSettings> buildTargetSettingsList = new List<BuildTargetSettings>();
            IList<FileSystemTreeNode> packageJsonFiles = treeAnalysis.FilesInfo[Constants.PackageJsonFileName];
            IList<FileSystemTreeNode> gulpFiles = treeAnalysis.FilesInfo[Constants.GulpFileName];

            foreach (FileSystemTreeNode packageJsonFile in packageJsonFiles)
            {
                foreach (FileSystemTreeNode gulpFile in gulpFiles)
                {
                    if (packageJsonFile.IsAncestorOf(gulpFile))
                    {
                        BuildTargetSettings buildTargetSetting = new BuildTargetSettings()
                        {
                            Name = BuildTargetName,
                            Settings = new Dictionary<string, object>()
                            {
                                { Constants.WorkingDirectory, packageJsonFile.GetDirectoryPath() },
                                { Constants.PackageJsonFilePath, packageJsonFile.Path },
                                { Constants.GulpFilePath, gulpFile.Path}
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
            if (treeAnalysis.FilesInfo.ContainsKey(Constants.GulpFileName))
            {
                return true;
            }
            return false;
        }
    }
}
