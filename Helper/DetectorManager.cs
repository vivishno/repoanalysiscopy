namespace GitHub.Services.RepositoryAnalysis.Detectors.Helpers
{
    using GitHub.Services.RepositoryAnalysis.Detectors.DeployTargetDetectors;
    using GitHub.Services.RepositoryAnalysis.Detectors.LanguageDetectors;
    using GitHub.Services.RepositoryAnalysis.Detectors.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Helper class.
    /// Has methods to import necessary resources particular to the detectors in this library.
    /// </summary>
    public class DetectorManager
    {
        /// <summary>
        /// Returns instances of all language detctors implemented in this library.
        /// </summary>
        /// <returns>List of ILanguageDetector objects</returns>
        public List<ILanguageDetector> ListLanguageDetectors()
        {
            List<ILanguageDetector> languageDetectors = new List<ILanguageDetector>();
            var type = typeof(ILanguageDetector);
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => p.IsAssignableFrom(type));

            foreach(var languageDetectorType in types)
            {
                languageDetectors.Add((ILanguageDetector)Activator.CreateInstance(languageDetectorType));
            }

            return languageDetectors;
        }

        /// <summary>
        /// Returns instances of all deploy target detctors implemented in this library.
        /// </summary>
        /// <returns>List of IDeployTargetDetector objects</returns>
        public List<IDeployTargetDetector> ListDeployTargetDetectors()
        {
            List<IDeployTargetDetector> deployTargetDetectors = new List<IDeployTargetDetector>();
            var type = typeof(IDeployTargetDetector);
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => p.IsAssignableFrom(type));

            foreach (var deployTargetDetectorType in types)
            {
                deployTargetDetectors.Add((IDeployTargetDetector)Activator.CreateInstance(deployTargetDetectorType));
            }

            return deployTargetDetectors;
        }

        /// <summary>
        /// Returns list of file regexes used in the detectors, to get those files read during treeAnalysis.
        /// </summary>
        /// <returns>List of regex strings</returns>
        public List<string> ListPatternsForFilesToBeRead()
        {
            return new List<string> { Constants.PackageJsonFileName };
        }
    }
}
