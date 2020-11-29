namespace GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using System.Collections.Generic;
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using GitHub.RepositoryAnalysis.Detectors.Utilities;

    public class DockerfileBuildTargetDetector : BuildTargetDetectorBase
    {
        public override string BuildTargetName => Constants.Dockerfile;

        public override List<BuildTargetSettings> GetBuildTargetSettings(TreeAnalysis treeAnalysis)
        {
            /*Logic: 
             * Working Directory:
             * dockerfile file path defines the working directory.
            */
            List<BuildTargetSettings> buildTargetSettingsList = new List<BuildTargetSettings>();
            Dictionary<string, object> settings = new Dictionary<string, object>();
            IList<FileSystemTreeNode> dockerfiles = treeAnalysis.FilesInfo[Constants.Dockerfile];
            foreach (FileSystemTreeNode node in dockerfiles)
            {
                BuildTargetSettings buildTargetSetting = new BuildTargetSettings()
                {
                    Name = BuildTargetName,
                    Settings = new Dictionary<string, object>()
                    {
                        { Constants.WorkingDirectory, node.GetDirectoryPath() },
                        { Constants.DockerFilePath, node.Path }
                    }
                };

                List<string> ports = this.GetPortNumberFromDockerFile(node.Path, treeAnalysis);
                if(ports.Count > 0)
                {
                    buildTargetSetting.Settings.Add(Constants.DockerPort, ports);
                }
                buildTargetSettingsList.Add(buildTargetSetting);
            }
            return buildTargetSettingsList;
        }

        public List<string> GetPortNumberFromDockerFile(string dockerFilePath, TreeAnalysis treeAnalysis)
        {
            if(treeAnalysis.FilesContent.ContainsKey(dockerFilePath))
            {
                var dockerFileContent = treeAnalysis.FilesContent[dockerFilePath].Value.ToString();
                var parser = new DockerFileParser(dockerFileContent);
                return parser.GetPortNumbers();
            }

            return new List<string>();
        }

        public override bool IsBuildTargetDetected(TreeAnalysis treeAnalysis)
        {
            //Check if file with the name settings.py exists along with manage.py file
            if (treeAnalysis.FilesInfo.ContainsKey(Constants.Dockerfile))
            {
                return true;
            }
            return false;
        }
    }
}