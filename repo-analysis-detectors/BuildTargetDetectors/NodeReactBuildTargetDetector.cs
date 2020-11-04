namespace Microsoft.VisualStudio.PortalExtension.Server.Services.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using Microsoft.VisualStudio.PortalExtension.Server.Services.RepositoryAnalysis.Detectors.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Build target detector for React build target for Node projects
    /// </summary>
    public class NodeReactBuildTargetDetector : BuildTargetDetectorBase
    {
        /// <summary>
        /// Build target name
        /// </summary>
        public override string BuildTargetName => Constants.ReactBuildTargetName;
        private List<string> ReactProjectFolders;

        /// <summary>
        /// Constructor for the build target detector
        /// </summary>
        public NodeReactBuildTargetDetector()
        {
            ReactProjectFolders = new List<string>();
        }

        /// <summary>
        /// Returns list of build target settings for the React build target.
        /// </summary>
        /// <param name="treeAnalysis"></param>
        /// <returns>List of build target settings for React</returns>
        public override List<BuildTargetSettings> GetBuildTargetSettings(TreeAnalysis treeAnalysis)
        {
            List<BuildTargetSettings> buildTargetSettings = new List<BuildTargetSettings>();
            foreach (var reactProjectFolder in ReactProjectFolders)
            {
                buildTargetSettings.Add(
                    new BuildTargetSettings
                    {
                        Name = BuildTargetName,
                        Settings = new Dictionary<string, object>
                        {
                            { Constants.WorkingDirectory, reactProjectFolder },
                            { Constants.AppArtifactLocation, $"{reactProjectFolder}/{Constants.ReactBuildArtifactPath}" },
                            { Constants.PackageJsonFilePath, $"{reactProjectFolder}/{Constants.PackageJsonFileName}" }
                        }
                    }
                );
            }
            return buildTargetSettings;
        }

        /// <summary>
        /// Checks if repo contains React build target projects, using treeAnalysis object.
        /// </summary>
        /// <param name="treeAnalysis"></param>
        /// <returns>true on React build target detection, otherwise false</returns>
        public override bool IsBuildTargetDetected(TreeAnalysis treeAnalysis)
        {
            FindAndAddReactProjectFolders(treeAnalysis);
            if (ReactProjectFolders.Count > 0)
                return true;
            return false;
        }

        internal void FindAndAddReactProjectFolders(TreeAnalysis treeAnalysis)
        {
            List<FileSystemTreeNode> packageJsonNodes = new List<FileSystemTreeNode>(treeAnalysis.FilesInfo[Constants.PackageJsonFileName]);

            foreach (var packageJsonNode in packageJsonNodes)
            {
                if (HasReactDependency(treeAnalysis.FilesContent[packageJsonNode.Path].ToObject<PackageJsonModel>()))
                    ReactProjectFolders.Add(GetDirectoryPath(packageJsonNode));
            }
        }

        internal bool HasReactDependency(PackageJsonModel packageJson)
        {
            if (packageJson.dependencies.ContainsKey(Constants.ReactPackageName))
                return true;
            return false;
        }
    }
}
