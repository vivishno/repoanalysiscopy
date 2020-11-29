namespace GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using GitHub.RepositoryAnalysis.Detectors.LanguageDetectors;
    using System.Collections.Generic;

    public class PythonPlainBuildTargetDetector : BuildTargetDetectorBase
    {
        public override string BuildTargetName => Constants.PlainApplication;

        public override bool IsBuildTargetDetected(TreeAnalysis treeAnalysis)
        {
            //Check if file with the name settings.py exists along with manage.py file
            if (new PythonLanguageDetector().IsLanguageDetected(treeAnalysis)
                && treeAnalysis.FilesInfo.ContainsKey(Constants.RequirementsFileName))
            {
                return true;
            }
            return false;
        }

        public override List<BuildTargetSettings> GetBuildTargetSettings(TreeAnalysis treeAnalysis)
        {
            /*Logic: 
             * Working Directory:
             * Requirements.txt file path defines the working directory.
             * 
             * Setup.py File present in same directory then only to honor the presence of setup.py file
            */

            List<BuildTargetSettings> buildTargetSettingsList = new List<BuildTargetSettings>();
            IList<FileSystemTreeNode> requirementsFiles = treeAnalysis.FilesInfo[Constants.RequirementsFileName];

            Dictionary<string, string> setupFileDirectoryNameMap = new Dictionary<string, string>();
            if (treeAnalysis.FilesInfo.ContainsKey(Constants.SetupFileName))
            {
                IList<FileSystemTreeNode> setupFiles = treeAnalysis.FilesInfo[Constants.SetupFileName];
                foreach (FileSystemTreeNode node in setupFiles)
                {
                    setupFileDirectoryNameMap.Add(node.GetDirectoryPath(), node.Path);
                }
            }

            HashSet<string> manageFileDirectoryNameSet = new HashSet<string>();
            if (treeAnalysis.FilesInfo.ContainsKey(Constants.ManageFileName))
            {
                IList<FileSystemTreeNode> manageFiles = treeAnalysis.FilesInfo[Constants.ManageFileName];
                foreach (FileSystemTreeNode node in manageFiles)
                {
                    manageFileDirectoryNameSet.Add(node.GetDirectoryPath());
                }
            }

            foreach (FileSystemTreeNode node in requirementsFiles)
            {
                if (!manageFileDirectoryNameSet.Contains(node.GetDirectoryPath()))
                {
                    BuildTargetSettings buildTargetSetting = new BuildTargetSettings()
                    {
                        Name = BuildTargetName,
                        Settings = new Dictionary<string, object>()
                        {
                            { Constants.WorkingDirectory, node.GetDirectoryPath() },
                            { Constants.RequirementsFilePath, node.Path }
                        }
                    };
                    if (setupFileDirectoryNameMap.ContainsKey(node.GetDirectoryPath()))
                    {
                        buildTargetSetting.Settings.Add(Constants.SetupFilePath, setupFileDirectoryNameMap[node.GetDirectoryPath()]);
                    }
                    buildTargetSettingsList.Add(buildTargetSetting);
                }
            }
            return buildTargetSettingsList;
        }
    }
}