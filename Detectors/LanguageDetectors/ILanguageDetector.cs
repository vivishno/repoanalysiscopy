namespace GitHub.RepositoryAnalysis.Detectors.LanguageDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors;
    using GitHub.RepositoryAnalysis.Detectors.DeployTargetDetectors;
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using Microsoft.VisualStudio.Services.PortalExtension.RepositoryAnalysis.WebApi.Contracts;
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
        /// Returns list of build target settings for the build target detected.
        /// </summary>
        /// <returns>List of BuildTargetSettings objects</returns>
        IList<BuildTargetSettings> GetBuildTargetSettingsList(TreeAnalysis treeAnalysis);

        /// <summary>
        /// Returns list of deploy target settings for the deploy target detected.
        /// </summary>
        /// <returns>List of DeployTargetSettings objects</returns>
        IList<DeployTargetSettings> GetDeployTargetSettingsList(TreeAnalysis treeAnalysis);

        /// <summary>
        /// Returns list of application settings based on build target and deploy target settings
        /// </summary>
        /// <returns>List of ApplicationSettings objects</returns>
        IList<ApplicationSettings> GetApplicationSettings(TreeAnalysis treeAnalysis);
    }
}