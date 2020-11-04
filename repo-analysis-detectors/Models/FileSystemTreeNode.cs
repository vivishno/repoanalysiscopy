// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemTreeNode.cs" company="Microsoft Corporation">
//   2012-2023, All rights reserved.
// </copyright>
// <summary>
//   Defines the FileSystemTreeNode resource.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.VisualStudio.PortalExtension.Server.Services.RepositoryAnalysis.Detectors.Models
{
    /// <summary>
    /// Node in the TreeAnalysis object.
    /// </summary>
    public class FileSystemTreeNode
    {
        /// <summary>
        /// File name including its extension
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Full path including file name
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// true if the node specifies a directory.
        /// </summary>
        public bool IsDirectory { get; set; }
    }
}
