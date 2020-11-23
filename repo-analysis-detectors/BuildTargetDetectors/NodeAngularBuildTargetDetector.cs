namespace GitHub.Services.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using System.Collections.Generic;
    using GitHub.Services.RepositoryAnalysis.Detectors.Models;

    /// <summary>
    /// Build target detector for Vue build target for Node projects
    /// </summary>
    public class NodeAngularBuildTargetDetector : BuildTargetDetectorBase
    {
        /// <summary>
        /// Build target name
        /// </summary>
        public override string BuildTargetName => Constants.AngularBuildTargetName;
        private List<string> AngularProjectFolders;

        /// <summary>
        /// Constructor for the build target detector
        /// </summary>
        public NodeAngularBuildTargetDetector()
        {
            AngularProjectFolders = new List<string>();
        }

        /// <summary>
        /// Returns list of build target settings for the Angular build target.
        /// </summary>
        /// <param name="treeAnalysis"></param>
        /// <returns>List of build target settings for Angular</returns>
        public override List<BuildTargetSettings> GetBuildTargetSettings(TreeAnalysis treeAnalysis)
        {
            List<BuildTargetSettings> buildTargetSettings = new List<BuildTargetSettings>();
            foreach (var angularProjectFolder in AngularProjectFolders)
            {
                buildTargetSettings.Add(
                    new BuildTargetSettings
                    {
                        Name = BuildTargetName,
                        Settings = new Dictionary<string, object>
                        {
                            { Constants.WorkingDirectory, angularProjectFolder },
                            { Constants.AppArtifactLocation, $"{angularProjectFolder}/{Constants.AngularBuildArtifactPath}" },
                            { Constants.PackageJsonFilePath, $"{angularProjectFolder}/{Constants.PackageJsonFileName}" }
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
            FindAndAddVueProjectFolders(treeAnalysis);
            if (AngularProjectFolders.Count > 0)
                return true;
            return false;
        }

        internal void FindAndAddVueProjectFolders(TreeAnalysis treeAnalysis)
        {
            List<FileSystemTreeNode> packageJsonNodes = new List<FileSystemTreeNode>(treeAnalysis.FilesInfo[Constants.PackageJsonFileName]);

            foreach (var packageJsonNode in packageJsonNodes)
            {
                if (HasAngularDependency(treeAnalysis.FilesContent[packageJsonNode.Path].ToObject<PackageJsonModel>()))
                    AngularProjectFolders.Add(packageJsonNode.GetDirectoryPath());
            }
        }

        internal bool HasAngularDependency(PackageJsonModel packageJson)
        {
            if (packageJson.dependencies.ContainsKey(Constants.AngularPackageName))
                return true;
            return false;
        }
    }
}
