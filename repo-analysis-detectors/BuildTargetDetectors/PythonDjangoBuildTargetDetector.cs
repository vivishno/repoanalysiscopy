namespace GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using System.Collections.Generic;

    public class PythonDjangoBuildTargetDetector : BuildTargetDetectorBase
    {
        public override string BuildTargetName => Constants.DjangoBuildTargetName;

        public override bool IsBuildTargetDetected(TreeAnalysis treeAnalysis)
        {
            //Check if file with the name settings.py exists along with manage.py file
            if (treeAnalysis.FilesInfo.ContainsKey(Constants.ManageFileName))
            {
                return true;
            }
            return false;
        }

        public override List<BuildTargetSettings> GetBuildTargetSettings(TreeAnalysis treeAnalysis)
        {
            /*Logic: 
             * Working Directory:
             * Manage.py file  path defines the working directory.
             * 
             * Requirements.txt or setup.py file should be present in same directory of manage.py file directory
             * Settings.py file should be present in same or sub-directory of manage.py file directory
            */

            List<BuildTargetSettings> buildTargetSettingsList = new List<BuildTargetSettings>();
            IList<FileSystemTreeNode> manageFiles = treeAnalysis.FilesInfo[Constants.ManageFileName];
            IList<FileSystemTreeNode> settingsFiles = new List<FileSystemTreeNode>();

            if (treeAnalysis.FilesInfo.ContainsKey(Constants.DjangoSettingModuleName))
            {
                settingsFiles = treeAnalysis.FilesInfo[Constants.DjangoSettingModuleName];
            }

            Dictionary<string, string> requirementFileDirectoryNameMap = new Dictionary<string, string>();
            if (treeAnalysis.FilesInfo.ContainsKey(Constants.RequirementsFileName))
            {
                IList<FileSystemTreeNode> requirementsFiles = treeAnalysis.FilesInfo[Constants.RequirementsFileName];
                foreach (FileSystemTreeNode node in requirementsFiles)
                {
                    requirementFileDirectoryNameMap.Add(node.GetDirectoryPath(), node.Path);
                }
            }

            Dictionary<string, string> setupFileDirectoryNameMap = new Dictionary<string, string>();
            if (treeAnalysis.FilesInfo.ContainsKey(Constants.SetupFileName))
            {
                IList<FileSystemTreeNode> setupFiles = treeAnalysis.FilesInfo[Constants.SetupFileName];
                foreach (FileSystemTreeNode node in setupFiles)
                {
                    setupFileDirectoryNameMap.Add(node.GetDirectoryPath(), node.Path);
                }
            }

            foreach (FileSystemTreeNode manageFile in manageFiles)
            {
                string manageFileDirectoryPath = manageFile.GetDirectoryPath();
                BuildTargetSettings buildTargetSetting = new BuildTargetSettings()
                {
                    Name = BuildTargetName,
                    Settings = new Dictionary<string, object>()
                            {
                                { Constants.WorkingDirectory, manageFileDirectoryPath },
                                { Constants.ManageFilePath, manageFile.Path },
                            }
                };

                if (requirementFileDirectoryNameMap.ContainsKey(manageFileDirectoryPath))
                {
                    buildTargetSetting.Settings[Constants.RequirementsFilePath] = requirementFileDirectoryNameMap[manageFileDirectoryPath];
                }

                if (setupFileDirectoryNameMap.ContainsKey(manageFileDirectoryPath))
                {
                    buildTargetSetting.Settings[Constants.SetupFilePath] = setupFileDirectoryNameMap[manageFileDirectoryPath];
                }

                foreach (FileSystemTreeNode settingsFile in settingsFiles)
                {
                    if (settingsFile.IsAncestorOf(manageFile))
                    {
                        buildTargetSetting.Settings[Constants.DjangoSettingModulePath] = settingsFile.Path;
                        settingsFiles.Remove(settingsFile);
                        break;
                    }
                }
                buildTargetSettingsList.Add(buildTargetSetting);
            }
            return buildTargetSettingsList;
        }
    }
}