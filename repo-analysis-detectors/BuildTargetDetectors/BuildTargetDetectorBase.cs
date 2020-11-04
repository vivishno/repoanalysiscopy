// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildTargetDetectorBase.cs" company="Microsoft Corporation">
//   2012-2023, All rights reserved.
// </copyright>
// <summary>
//   Base class for Build Detector
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.VisualStudio.PortalExtension.Server.Services.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.VisualStudio.PortalExtension.Server.Services.RepositoryAnalysis.Detectors.Models;

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

        /// <summary>
        /// Checks if first parameter node is an ancestor of second parameter node.
        /// </summary>
        /// <param name="ancestor"></param>
        /// <param name="child"></param>
        /// <returns>true if "ancestor" is indeed an ancestor of "child"</returns>
        public bool IsAncestor(FileSystemTreeNode ancestor, FileSystemTreeNode child)
        {
            return (Path.GetDirectoryName(child.Path) + Path.DirectorySeparatorChar)
                .Contains(Path.GetDirectoryName(ancestor.Path) + Path.DirectorySeparatorChar);
        }
    }
}