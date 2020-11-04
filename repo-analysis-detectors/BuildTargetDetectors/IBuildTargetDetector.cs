// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBuildTargetDetector.cs" company="Microsoft Corporation">
//   2012-2023, All rights reserved.
// </copyright>
// <summary>
//    Defines the interface for Repo Build Detectors.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.VisualStudio.PortalExtension.Server.Services.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using Microsoft.VisualStudio.PortalExtension.Server.Services.RepositoryAnalysis.Detectors.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Base interface for language detectors
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
