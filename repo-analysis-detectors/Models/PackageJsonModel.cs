namespace GitHub.RepositoryAnalysis.Detectors.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Model for Package.json schema
    /// </summary>
    public class PackageJsonModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PackageJsonModel()
        {
            dependencies = new Dictionary<string, string>();
            devDependencies = new Dictionary<string, string>();
        }

        /// <summary>
        /// Name of the project.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Project version.
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// Dependencies from package.json, stored as dictionary.
        /// for eg, "react": "^16.15.0"
        /// </summary>
        public IDictionary<string, string> dependencies { get; set; }

        /// <summary>
        /// DevDependencies from package.json, stored as dictionary.
        /// for eg, "eslint": "^7.12.0"
        /// </summary>
        public IDictionary<string, string> devDependencies { get; set; }
    }
}
