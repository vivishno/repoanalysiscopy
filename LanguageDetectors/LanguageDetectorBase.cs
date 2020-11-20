namespace GitHub.Services.RepositoryAnalysis.Detectors.LanguageDetectors
{
    using System;
    using System.Collections.Generic;
    using GitHub.Services.RepositoryAnalysis.Detectors.BuildTargetDetectors;
    using GitHub.Services.RepositoryAnalysis.Detectors.DeployTargetDetectors;
    using GitHub.Services.RepositoryAnalysis.Detectors.Models;

    /// <summary>
    /// Base detector abstract class for language detectors.
    /// Extended in every language detector.
    /// </summary>
    public abstract class LanguageDetectorBase : ILanguageDetector
    {
        /// <summary>
        /// Language target name
        /// </summary>
        public virtual string Language => throw new NotImplementedException();

        /// <summary>
        /// To be implemented by the language detector.
        /// Identifies if particular language is detected in the repo, based on the detection criteria involving the treeAnalysis object.
        /// </summary>
        /// <param name="treeAnalysis"></param>
        /// <returns>true on language detection, otherwise false</returns>
        public abstract bool IsLanguageDetected(TreeAnalysis treeAnalysis);

        /// <summary>
        /// Returns list of build target detector instances for the language detected.
        /// </summary>
        /// <returns>List of IBuildTargetDetector objects</returns>
        public IList<IBuildTargetDetector> GetBuildTargetDetectors()
        {
            switch (Language)
            {
                case Constants.NodeLanguageName:
                    return new List<IBuildTargetDetector>
                    {
                        new NodeReactBuildTargetDetector(),
                        new NodeVueBuildTargetDetector()
                    };
            }
            return new List<IBuildTargetDetector> { };
        }

        /// <summary>
        /// Returns list of deploy target detector instances for the language detected.
        /// </summary>
        /// <returns>List of IDeployTargetDetector objects</returns>
        public IList<IDeployTargetDetector> GetDeployTargetDetectors()
        {
            return new List<IDeployTargetDetector> { };
        }
    }
}
