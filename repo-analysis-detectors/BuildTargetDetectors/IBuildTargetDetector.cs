namespace GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Base interface for build target detectors
    /// </summary>
    public interface IBuildTargetDetector
    {

        /// <summary>
        /// Interface property for Build Target name
        /// </summary>
        string BuildTargetName { get; }

        /// <summary>
        /// Return bool if implemented build target detector has detected itself or not
        /// </summary>
        /// <param name="treeAnalysis"></param>
        bool IsBuildTargetDetected(TreeAnalysis treeAnalysis);

        /// <summary>
        /// Return Build Settings
        /// </summary>
        /// <param name="treeAnalysis"></param>
        /// <returns></returns>
        List<BuildTargetSettings> GetBuildTargetSettings(TreeAnalysis treeAnalysis);
    }
}
