namespace GitHub.Services.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GitHub.Services.RepositoryAnalysis.Detectors.Models;

    /// <summary>
    /// Base abstract class for build target detectors.
    /// </summary>
    public abstract class BuildTargetDetectorBase : IBuildTargetDetector
    {
        /// <summary>
        /// Base build target constructor
        /// </summary>
        public BuildTargetDetectorBase()
        {

        }

        /// <summary>
        /// Build target name
        /// </summary>
        public virtual string BuildTargetName => throw new NotImplementedException();

        /// <summary>
        /// To be implemented by the build target detector.
        /// Identifies if particular build target is detected in the repo, based on the detection criteria involving the treeAnalysis object.
        /// </summary>
        /// <param name="treeAnalysis"></param>
        /// <returns>true on build target detection, otherwise false</returns>
        public abstract bool IsBuildTargetDetected(TreeAnalysis treeAnalysis);

        /// <summary>
        /// To be implemented by the build target detector.
        /// Returns list of build target settings for the build target already found in the repo.
        /// </summary>
        /// <param name="treeAnalysis"></param>
        /// <returns>List of build target settings</returns>
        public abstract List<BuildTargetSettings> GetBuildTargetSettings(TreeAnalysis treeAnalysis);

        /// <summary>
        /// Returns directory path for a string
        /// </summary>
        /// <param name="nodePath"></param>
        /// <returns>Directory level path</returns>
        public string GetDirectoryPathFromString(string nodePath)
        {
            if (string.IsNullOrEmpty(System.IO.Path.GetDirectoryName(nodePath)))
            {
                return ".";
            }
            string[] dirs = nodePath.Split('/');
            return String.Join("/", dirs.Take(dirs.Length - 1));
        }
    }
}