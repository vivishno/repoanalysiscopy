namespace GitHub.RepositoryAnalysis.Detectors.DeployTargetDetectors
{
    using System.Linq;
    using System.Collections.Generic;
    using GitHub.RepositoryAnalysis.Detectors.Models;

    public class AKSHelmChartDetector : DeployTargetDetectorBase
    {
        public override string DeployTargetName => Constants.AKSHelmChart;

        public override bool IsDeployTargetDetected(TreeAnalysis treeAnalysis)
        {
            return treeAnalysis.FilesInfo.ContainsKey(Constants.chartFileName) && treeAnalysis.DirectoryInfo.ContainsKey(Constants.templatesDirectoryName);
        }

        public override List<DeployTargetSettings> GetDeployTargetSettings(TreeAnalysis treeAnalysis)
        {
            /*Logic: 
             * Working Directory:
             * A directory with Chart.yaml and templates directory is a working Helm Chart Directory.
            */
            List<DeployTargetSettings> deployTargetSettingsList = new List<DeployTargetSettings>();
            HashSet<FileSystemTreeNode> chartFiles = new HashSet<FileSystemTreeNode>(treeAnalysis.FilesInfo[Constants.chartFileName]);
            HashSet<string> chartDirectoryPaths = convertToDirectoryPathsSet(chartFiles);

            HashSet<FileSystemTreeNode> templatesDirectories = new HashSet<FileSystemTreeNode>(treeAnalysis.DirectoryInfo[Constants.templatesDirectoryName]);
            HashSet<string> templatesDirectoryPaths = convertToDirectoryPathsSet(templatesDirectories);

            List<string> directoriesContainingAllFiles = getCommonWorkingDirectories(new[] { chartDirectoryPaths, templatesDirectoryPaths });
            foreach (string directory in directoriesContainingAllFiles)
            {
                DeployTargetSettings deployTargetSetting = new DeployTargetSettings()
                {
                    Name = DeployTargetName,
                    Settings = new Dictionary<string, object>()
                    {
                        { Constants.WorkingDirectory, GetDirectoryPathFromString(directory) },
                        { Constants.helmChartPath, directory }
                    }
                };
                deployTargetSettingsList.Add(deployTargetSetting);
            }
            return deployTargetSettingsList;
        }

        internal List<string> getCommonWorkingDirectories(HashSet<string>[] filesDirectoryPaths)
        {
            if (filesDirectoryPaths == null || filesDirectoryPaths.Length == 0)
            {
                return new List<string>();
            }
            HashSet<string> DirectoriesContainingAllFiles = filesDirectoryPaths.FirstOrDefault();
            foreach (HashSet<string> filesDirectoryPath in filesDirectoryPaths)
            {
                DirectoriesContainingAllFiles.IntersectWith(filesDirectoryPath);
            }
            return new List<string>(DirectoriesContainingAllFiles);
        }

        private HashSet<string> convertToDirectoryPathsSet(HashSet<FileSystemTreeNode> fileNodesSet)
        {
            HashSet<string> directoryPaths = new HashSet<string>();
            foreach (FileSystemTreeNode node in fileNodesSet)
            {
                string path = node.GetDirectoryPath();
                directoryPaths.Add(path);
            }
            return directoryPaths;
        }
    }
}

