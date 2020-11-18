namespace GitHub.Services.RepositoryAnalysis.Detectors.DeployTargetDetectors
{
    using GitHub.Services.RepositoryAnalysis.Detectors.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Base interface for deploy target detectors
    /// </summary>
    public interface IDeployTargetDetector
    {
        /// <summary>
        /// Interface property for Deploy Target name
        /// </summary>
        string DeployTargetName { get; }

        /// <summary>
        /// Return bool if implemented Deploy target detector has detected itself or not
        /// </summary>
        /// <param name="treeAnalysis"></param>
        bool IsDeployTargetDetected(TreeAnalysis treeAnalysis);

        /// <summary>
        /// Return list of Deploy targets detected by its detector class
        /// </summary>
        /// <param name="treeAnalysis"></param>
        List<DeployTargetSettings> GetDeployTargetSettings(TreeAnalysis treeAnalysis);
    }
}