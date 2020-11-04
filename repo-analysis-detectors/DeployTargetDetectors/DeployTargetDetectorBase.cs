// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeployTargetDetectorBase.cs" company="Microsoft Corporation">
//   2012-2023, All rights reserved.
// </copyright>
// <summary>
//    Base class for deploy target detectors.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.VisualStudio.PortalExtension.Server.Services.RepositoryAnalysis.Detectors.DeployTargetDetectors
{
    using Microsoft.VisualStudio.PortalExtension.Server.Services.RepositoryAnalysis.Detectors.Models;
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
        /// Finds parent directory name for the node from treeanalysis object.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Parent directory name</returns>
        public string GetDirectoryPath(FileSystemTreeNode node)
        {
            return GetDirectoryPath(node.Path);
        }

        /// <summary>
        /// Finds parent directory name for the node from treeanalysis object.
        /// </summary>
        /// <param name="nodePath"></param>
        /// <returns>Parent directory name</returns>
        public string GetDirectoryPath(string nodePath)
        {
            if (string.IsNullOrEmpty(Path.GetDirectoryName(nodePath)))
            {
                return ".";
            }
            string[] dirs = nodePath.Split('/');
            return String.Join("/", dirs.Take(dirs.Length - 1));
        }
    }
}
