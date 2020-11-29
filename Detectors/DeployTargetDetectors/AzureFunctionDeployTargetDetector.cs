namespace GitHub.RepositoryAnalysis.Detectors.DeployTargetDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using System.Collections.Generic;

    public class AzureFunctionDeployTargetDetector : DeployTargetDetectorBase
    {
        public override string DeployTargetName => Constants.AzureFunctionName;

        public override bool IsDeployTargetDetected(TreeAnalysis treeAnalysis)
        {
            if (treeAnalysis.FilesInfo.ContainsKey(Constants.HostFileName))
            {
                return true;
            }
            return false;
        }

        public override List<DeployTargetSettings> GetDeployTargetSettings(TreeAnalysis treeAnalysis)
        {
            /*Logic: 
             * Working Directory:
             * host.json file path defines the working directory.
            */
            List<DeployTargetSettings> deployTargetSettingsList = new List<DeployTargetSettings>();
            IList<FileSystemTreeNode> hostJsonFiles = treeAnalysis.FilesInfo[Constants.HostFileName];

            foreach (FileSystemTreeNode node in hostJsonFiles)
            {
                DeployTargetSettings deployTargetSetting = new DeployTargetSettings()
                {
                    Name = DeployTargetName,
                    Settings = new Dictionary<string, object>()
                    {
                        { Constants.WorkingDirectory, node.GetDirectoryPath() },
                        { Constants.HostFilePath, node.Path }
                    }
                };
                deployTargetSettingsList.Add(deployTargetSetting);
            }
            return deployTargetSettingsList;
        }
    }
}