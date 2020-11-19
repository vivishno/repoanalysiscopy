namespace GitHub.Services.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using System.Collections.Generic;
    using GitHub.Services.RepositoryAnalysis.Detectors.Models;

    /// <summary>
    /// Build target detector for Vue build target for Node projects
    /// </summary>
    public class NodeVueBuildTargetDetector : BuildTargetDetectorBase
    {
        /// <summary>
        /// Build target name
        /// </summary>
        public override string BuildTargetName => Constants.VueBuildTargetName;
        private List<string> VueProjectFolders;

        /// <summary>
        /// Constructor for the build target detector
        /// </summary>
        public NodeVueBuildTargetDetector()
        {
            VueProjectFolders = new List<string>();
        }

        /// <summary>
        /// Returns list of build target settings for the Vue build target.
        /// </summary>
        /// <param name="treeAnalysis"></param>
        /// <returns>List of build target settings for Vue</returns>
        public override List<BuildTargetSettings> GetBuildTargetSettings(TreeAnalysis treeAnalysis)
        {
            List<BuildTargetSettings> buildTargetSettings = new List<BuildTargetSettings>();
            foreach (var vueProjectFolder in VueProjectFolders)
            {
                buildTargetSettings.Add(
                    new BuildTargetSettings
                    {
                        Name = BuildTargetName,
                        Settings = new Dictionary<string, object>
                        {
                            { Constants.WorkingDirectory, vueProjectFolder },
                            { Constants.AppArtifactLocation, $"{vueProjectFolder}/{Constants.VueBuildArtifactPath}" },
                            { Constants.PackageJsonFilePath, $"{vueProjectFolder}/{Constants.PackageJsonFileName}" }
                        }
                    }
                );
            }
            return buildTargetSettings;
        }

        /// <summary>
        /// Checks if repo contains Vue build target projects, using treeAnalysis object.
        /// </summary>
        /// <param name="treeAnalysis"></param>
        /// <returns>true on Vue build target detection, otherwise false</returns>
        public override bool IsBuildTargetDetected(TreeAnalysis treeAnalysis)
        {
            PopulateVueProjectDirectories(treeAnalysis);
            if (VueProjectFolders.Count > 0)
                return true;
            return false;
        }

        internal void PopulateVueProjectDirectories(TreeAnalysis treeAnalysis)
        {
            List<FileSystemTreeNode> packageJsonNodes = new List<FileSystemTreeNode>(treeAnalysis.FilesInfo[Constants.PackageJsonFileName]);

            foreach (var packageJsonNode in packageJsonNodes)
            {
                if (HasVueDependency(treeAnalysis.FilesContent[packageJsonNode.Path].ToObject<PackageJsonModel>()))
                    VueProjectFolders.Add(packageJsonNode.GetDirectoryPath());
            }
        }

        internal bool HasVueDependency(PackageJsonModel packageJson)
        {
            if (packageJson.dependencies.ContainsKey(Constants.VuePackageName))
                return true;
            return false;
        }
    }
}
