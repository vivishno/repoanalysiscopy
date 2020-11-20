namespace GitHub.Services.RepositoryAnalysis.Detectors.Models
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Output of deploy target detection.
    /// Encapsulates deploy target name and other settings particular to the build target.
    /// </summary>
    public class DeployTargetSettings
    {
        /// <summary>
        /// Name of the deploy target.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Settings found from deploy target detection, stored as a Dictionary
        /// </summary>
        public Dictionary<string, object> Settings { get; set; }

        /// <summary>
        /// Returns non-null entries from "Settings" as a Dictionary
        /// </summary>
        /// <returns>Dictionary</returns>
        public Dictionary<string, object> GetNonNullSettings()
        {
            return Settings.Where(entry => entry.Value != null).ToDictionary(x => x.Key, x => x.Value); ;
        }
    }
}
