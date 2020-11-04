// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildTargetSettings.cs" company="Microsoft Corporation">
//   2012-2023, All rights reserved.
// </copyright>
// <summary>
//   Defines the FileSystemTreeNode resource.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.VisualStudio.PortalExtension.Server.Services.RepositoryAnalysis.Detectors.Models
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Output of build target detection.
    /// Encapsulates Build target name and other settings particular to the build target.
    /// </summary>
    public class BuildTargetSettings
    {
        /// <summary>
        /// Name of the build target.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Settings found from build target detection, stored as a Dictionary
        /// </summary>
        public Dictionary<string, object> Settings { get; set; }

        /// <summary>
        /// Returns non-null entries from "Settings" as a Dictionary
        /// </summary>
        /// <returns>Dictionary</returns>
        public Dictionary<string, object> GetNonNullSettings()
        {
            return Settings.Where(entry => entry.Value != null).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
