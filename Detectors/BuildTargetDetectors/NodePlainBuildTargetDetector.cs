namespace GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using GitHub.RepositoryAnalysis.Detectors.LanguageDetectors;
    using System.Collections.Generic;

    public class NodePlainBuildTargetDetector : BuildTargetDetectorBase
    {
        public override string BuildTargetName => Constants.PlainApplication;

        public override List<BuildTargetSettings> GetBuildTargetSettings(TreeAnalysis treeAnalysis)
        {
            /*Logic: 
             * Working Directory:
             * Package.json file path defines the working directory.
            */
            List<BuildTargetSettings> buildTargetSettingsList = new List<BuildTargetSettings>();
            IList <FileSystemTreeNode> packageJsonFiles = treeAnalysis.FilesInfo[Constants.PackageJsonFileName];

            HashSet<string> gulpFileDirectoryNameSet = new HashSet<string>();
            if (treeAnalysis.FilesInfo.ContainsKey(Constants.GulpFileName))
            {
                IList<FileSystemTreeNode> gulpFiles = treeAnalysis.FilesInfo[Constants.GulpFileName];
                foreach (FileSystemTreeNode node in gulpFiles)
                {
                    gulpFileDirectoryNameSet.Add(node.GetDirectoryPath());
                }
            }

            HashSet<string> gruntFileDirectoryNameSet = new HashSet<string>();
            if (treeAnalysis.FilesInfo.ContainsKey(Constants.GruntFileName))
            {
                IList<FileSystemTreeNode> gruntFiles = treeAnalysis.FilesInfo[Constants.GruntFileName];
                foreach (FileSystemTreeNode node in gruntFiles)
                {
                    gruntFileDirectoryNameSet.Add(node.GetDirectoryPath());
                }
            }

            foreach (FileSystemTreeNode node in packageJsonFiles)
            {
                string packageJsonFileDirectoryPath = node.GetDirectoryPath();
                if (!gulpFileDirectoryNameSet.Contains(packageJsonFileDirectoryPath)
                    && !gruntFileDirectoryNameSet.Contains(packageJsonFileDirectoryPath))
                {
                    BuildTargetSettings buildTargetSetting = new BuildTargetSettings()
                    {
                        Name = BuildTargetName,
                        Settings = new Dictionary<string, object>()
                    {
                        { Constants.WorkingDirectory, packageJsonFileDirectoryPath },
                        { Constants.PackageJsonFilePath, node.Path }
                    }
                    };
                    buildTargetSettingsList.Add(buildTargetSetting);
                }
            }
            return buildTargetSettingsList;
        }

        public override bool IsBuildTargetDetected(TreeAnalysis treeAnalysis)
        {
            if (new NodeLanguageDetector().IsLanguageDetected(treeAnalysis))
            {
                return true;
            }
            return false;
        }
    }
}