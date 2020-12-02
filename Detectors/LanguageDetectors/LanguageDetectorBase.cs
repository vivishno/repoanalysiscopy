namespace GitHub.RepositoryAnalysis.Detectors.LanguageDetectors
{
    using System;
    using System.Collections.Generic;
    using GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors;
    using GitHub.RepositoryAnalysis.Detectors.DeployTargetDetectors;
    using GitHub.RepositoryAnalysis.Detectors.Models;

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
        /// Returns list of build target settings for the build target detected.
        /// </summary>
        /// <returns>List of BuildTargetSettings objects</returns>
        public IList<BuildTargetSettings> GetBuildTargetSettingsList(TreeAnalysis treeAnalysis)
        {
            List<BuildTargetSettings> buildTargetSettingsList = new List<BuildTargetSettings>();

            IList<IBuildTargetDetector> buildTargetDetectors = GetBuildTargetDetectors();
            foreach (IBuildTargetDetector detector in buildTargetDetectors)
            {
                if (!detector.IsBuildTargetDetected(treeAnalysis))
                {
                    continue;
                }
                buildTargetSettingsList.AddRange(detector.GetBuildTargetSettings(treeAnalysis));
            }
            return buildTargetSettingsList;
        }

        /// <summary>
        /// Returns list of deploy target settings for the deploy target detected.
        /// </summary>
        /// <returns>List of DeployTargetSettings objects</returns>
        public IList<DeployTargetSettings> GetDeployTargetSettingsList(TreeAnalysis treeAnalysis)
        {
            List<DeployTargetSettings> deployTargetSettingsList = new List<DeployTargetSettings>();

            IList<IDeployTargetDetector> deployTargetDetectors = GetDeployTargetDetectors();
            foreach (IDeployTargetDetector detector in deployTargetDetectors)
            {
                if (!detector.IsDeployTargetDetected(treeAnalysis))
                {
                    continue;
                }
                deployTargetSettingsList.AddRange(detector.GetDeployTargetSettings(treeAnalysis));
            }
            return deployTargetSettingsList;
        }

        /// <summary>
        /// Analyzes the repository and returns application settings based on the findings
        /// </summary>
        /// <returns>List of ApplicationSettings objects</returns>
        public IList<ApplicationSettings> GetApplicationSettings(TreeAnalysis treeAnalysis)
        {
            IList<BuildTargetSettings> buildTargetSettings = GetBuildTargetSettingsList(treeAnalysis);
            IList<DeployTargetSettings> deployTargetSettings = GetDeployTargetSettingsList(treeAnalysis);
            return GetApplicationSettings(buildTargetSettings, deployTargetSettings);
        }

        /// <summary>
        /// Returns list of build target detector instances for the language detected.
        /// </summary>
        /// <returns>List of IBuildTargetDetector objects</returns>
        private IList<IBuildTargetDetector> GetBuildTargetDetectors()
        {
            switch (Language)
            {
                case Constants.PythonLanguageName:
                    return new List<IBuildTargetDetector>
                    {
                        new PythonDjangoBuildTargetDetector(),
                        new PythonPlainBuildTargetDetector()
                    };
                case Constants.NodeLanguageName:
                    return new List<IBuildTargetDetector>
                    {
                        new NodeGulpBuildTargetDetector(),
                        new NodeGruntBuildTargetDetector(),
                        new NodePlainBuildTargetDetector(),
                        new NodeReactBuildTargetDetector(),
                        new NodeVueBuildTargetDetector(),
                        new NodeAngularBuildTargetDetector()
                    };
                case Constants.Docker:
                    return new List<IBuildTargetDetector>
                    {
                        new DockerfileBuildTargetDetector(),
                    };
                case Constants.DotNetFramework:
                    return new List<IBuildTargetDetector>
                    {
                        new DotNetFrameworkWebBuildTargetDetector(),
                        new DotNetFrameworkConsoleBuildDetector(),
                        new DotNetFrameworkLibraryBuildTargetDetector(),
                        new DotNetFrameworkPlainBuildTargetDetector()
                    };
                case Constants.DotNetCore:
                    return new List<IBuildTargetDetector>
                    {
                        new DotNetCoreWebBuildTargetDetector(),
                        new DotNetCoreWorkerBuildTargetDetector(),
                        new DotNetCoreConsoleBuildTargetDetector()
                    };
            }
            return new List<IBuildTargetDetector> { };
        }

        /// <summary>
        /// Returns list of deploy target detector instances for the language detected.
        /// </summary>
        /// <returns>List of IDeployTargetDetector objects</returns>
        private IList<IDeployTargetDetector> GetDeployTargetDetectors()
        {
            switch (Language)
            {
                case Constants.PythonLanguageName:
                    return new List<IDeployTargetDetector>
                    {
                        new AzureFunctionDeployTargetDetector()
                    };
                case Constants.NodeLanguageName:
                    return new List<IDeployTargetDetector>
                    {
                        new AzureFunctionDeployTargetDetector()
                    };
                case Constants.Docker:
                    return new List<IDeployTargetDetector>
                    {
                        new AKSHelmChartDetector()
                    };
            }
            return new List<IDeployTargetDetector> { };
        }

        /// <summary>
        /// Returns list of application settings based on build target and deploy target settings
        /// </summary>
        /// <returns>List of ApplicationSettings objects</returns>
        private IList<ApplicationSettings> GetApplicationSettings(IList<BuildTargetSettings> buildSettingsList, IList<DeployTargetSettings> deploySettingsList)
        {
            List<ApplicationSettings> languageAnalysisList = new List<ApplicationSettings>();
            if (buildSettingsList.Count == 0 && deploySettingsList.Count > 0)
            {
                ApplicationSettings applicationAnalysis = new ApplicationSettings();
                applicationAnalysis.Language = Language;
                foreach (DeployTargetSettings deploySettings in deploySettingsList)
                {
                    applicationAnalysis.DeployTargetName = deploySettings.Name;
                    applicationAnalysis.Settings = deploySettings.GetNonNullSettings();
                    languageAnalysisList.Add(applicationAnalysis);
                }
            }
            else
            {
                foreach (BuildTargetSettings buildSettings in buildSettingsList)
                {
                    bool matchingDeploySettingFound = false;
                    foreach (DeployTargetSettings deploySettings in deploySettingsList)
                    {
                        ApplicationSettings languageAnalysis = getBuildLanguageAnalysis(buildSettings, Language);
                        if (String.Equals(buildSettings.Settings[Constants.WorkingDirectory], deploySettings.Settings[Constants.WorkingDirectory]))
                        {
                            matchingDeploySettingFound = true;
                            languageAnalysis.DeployTargetName = deploySettings.Name;
                            foreach (KeyValuePair<string, object> entry in deploySettings.GetNonNullSettings())
                            {
                                if (!languageAnalysis.Settings.ContainsKey(entry.Key))
                                {
                                    languageAnalysis.Settings[entry.Key] = entry.Value;
                                }
                            }
                            languageAnalysisList.Add(languageAnalysis);
                        }
                    }
                    if (!matchingDeploySettingFound)
                    {
                        languageAnalysisList.Add(getBuildLanguageAnalysis(buildSettings, Language));
                    }
                }
            }
            return languageAnalysisList;
        }

        private ApplicationSettings getBuildLanguageAnalysis(BuildTargetSettings buildSettings, string Language)
        {
            ApplicationSettings languageAnalysis = new ApplicationSettings();
            languageAnalysis.Language = Language;
            languageAnalysis.BuildTargetName = buildSettings.Name;
            languageAnalysis.Settings = buildSettings.GetNonNullSettings();
            return languageAnalysis;
        }
    }
}
