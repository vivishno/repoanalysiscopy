﻿namespace GitHub.RepositoryAnalysis.Detectors.Helpers
{
    using GitHub.RepositoryAnalysis.Detectors.LanguageDetectors;
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using System.Collections.Generic;

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
            return new List<ILanguageDetector> {
                new PythonLanguageDetector(),
                new NodeLanguageDetector(),
                new DockerDetector(),
                new DotNetLanguageDetector(),
                new DotNetCoreLanguageDetector()
            };
        }

        /// <summary>
        /// Returns list of file regexes used in the detectors, to get those files read during treeAnalysis.
        /// </summary>
        /// <returns>List of regex strings</returns>
        public List<string> ListPatternsForFilesToBeRead()
        {
            return new List<string> { Constants.PackageJsonFileName, Constants.CsProjDetectionRegex, Constants.GlobalJsonParseRegex, "^.*Dockerfile.*$" };
        }
    }
}
