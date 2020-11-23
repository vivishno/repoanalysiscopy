namespace GitHub.Services.RepositoryAnalysis.Detectors.DeployTargetDetectors
{
    using GitHub.Services.RepositoryAnalysis.Detectors.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Base abstract class for deploy target detectors.
    /// </summary>
    public abstract class DeployTargetDetectorBase : IDeployTargetDetector
    {
        /// <summary>
        /// Deploy target name.
        /// </summary>
        public virtual string DeployTargetName => throw new NotImplementedException();

        /// <summary>
        /// To be implemented by the deploy target detector.
        /// Identifies if particular deploy target is detected in the repo, based on the detection criteria involving the treeAnalysis object.
        /// </summary>
        /// <param name="treeAnalysis"></param>
        /// <returns>true on deploy target detection, otherwise false</returns>
        public abstract bool IsDeployTargetDetected(TreeAnalysis treeAnalysis);

        /// <summary>
        /// To be implemented by the deploy target detector.
        /// Returns list of deploy target settings for the deploy target already found in the repo.
        /// </summary>
        /// <param name="treeAnalysis"></param>
        /// <returns>List of deploy target settings</returns>
        public abstract List<DeployTargetSettings> GetDeployTargetSettings(TreeAnalysis treeAnalysis);

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
