namespace GitHub.RepositoryAnalysis.Detectors.LanguageDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors;
    using GitHub.RepositoryAnalysis.Detectors.DeployTargetDetectors;
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Base language detctor interface.
    /// </summary>
    public interface ILanguageDetector
    {
        /// <summary>
        /// Interface property for Language detector
        /// </summary>
        string Language { get; }

        /// <summary>
        /// Check if language detected or not
        /// </summary>
        /// <param name="treeAnalysis"></param>
        bool IsLanguageDetected(TreeAnalysis treeAnalysis);

        /// <summary>
        /// Return build target detector instances for given language detector.
        /// </summary>
        /// <returns>List of IBuildTargetDetector objects</returns>
        IList<IBuildTargetDetector> GetBuildTargetDetectors();

        /// <summary>
        /// Returns deploy target detector instances for given language detector.
        /// </summary>
        /// <returns>List of IDeployTargetDetector objects</returns>
        IList<IDeployTargetDetector> GetDeployTargetDetectors();
    }
}