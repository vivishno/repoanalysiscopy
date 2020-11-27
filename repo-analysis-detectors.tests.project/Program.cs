namespace repo_analysis_detectors.tests.project
{
    using repo_analysis_detectors.tests.project.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using GitHub.RepositoryAnalysis.Detectors.LanguageDetectors;
    using System;
    using System.Linq;
    using GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors;
    using GitHub.RepositoryAnalysis.Detectors.DeployTargetDetectors;
    using System.Text.Json;

    class Program
    {
        async static Task Main(string[] args)
        {
            TreeAnalysisService treeAnalysisService = new TreeAnalysisService();

            SourceRepository sourceRepository = new SourceRepository
            {
                Repository = new CodeRepository
                {
                    Id = "NinadKavimandan/hello-worlds", // you can use your own repo for local testing
                    DefaultBranch = "master" // same goes for branch
                }
            };

            sourceRepository.Repository.AuthorizationInfo = new Authorization();
            sourceRepository.Repository.AuthorizationInfo.Parameters = new Dictionary<string, string>();
            sourceRepository.Repository.AuthorizationInfo.Parameters.Add("AccessToken", "<USE_YOUR_OWN_PAT>");

            var treeAnalysis = await treeAnalysisService.GetTreeAnalysis(sourceRepository);

            var results = StartE2EFlow(treeAnalysis);
            Console.WriteLine(JsonSerializer.Serialize(results));
            Console.ReadKey();
        }

        private static List<ApplicationSettings> StartE2EFlow(TreeAnalysis treeAnalysis)
        {
            List<ApplicationSettings> applicationSettingsList = new List<ApplicationSettings>();
            IReadOnlyList<ILanguageDetector> languageDetectors = GetLanguageDetectors();

            foreach (var detector in languageDetectors)
            {
                if (!detector.IsLanguageDetected(treeAnalysis))
                {
                    continue;
                }

                IList<BuildTargetSettings> buildTargetSettingsList = GetBuildTargetSettingsList(detector, treeAnalysis);
                IList<DeployTargetSettings> deployTargetSettingsList = GetDeployTargetSettingsList(detector, treeAnalysis);
                List<ApplicationSettings> detectorSettingsList = GetApplicationSettings(detector.Language, buildTargetSettingsList, deployTargetSettingsList);
                applicationSettingsList.AddRange(detectorSettingsList);
            }

            return applicationSettingsList;
        }

        public static List<ApplicationSettings> GetApplicationSettings(string language, IList<BuildTargetSettings> buildSettingsList, IList<DeployTargetSettings> deploySettingsList)
        {
            List<ApplicationSettings> languageAnalysisList = new List<ApplicationSettings>();
            if (buildSettingsList.Count == 0 && deploySettingsList.Count > 0)
            {
                ApplicationSettings applicationAnalysis = new ApplicationSettings();
                applicationAnalysis.Language = language;
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
                        ApplicationSettings languageAnalysis = getBuildLanguageAnalysis(buildSettings, language);
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
                        languageAnalysisList.Add(getBuildLanguageAnalysis(buildSettings, language));
                    }
                }
            }
            return languageAnalysisList;
        }

        private static ApplicationSettings getBuildLanguageAnalysis(BuildTargetSettings buildSettings, string Language)
        {
            ApplicationSettings languageAnalysis = new ApplicationSettings();
            languageAnalysis.Language = Language;
            languageAnalysis.BuildTargetName = buildSettings.Name;
            languageAnalysis.Settings = buildSettings.GetNonNullSettings();
            return languageAnalysis;
        }

        private static IList<BuildTargetSettings> GetBuildTargetSettingsList(ILanguageDetector languageDetector, TreeAnalysis treeAnalysis)
        {
            List<BuildTargetSettings> buildTargetSettingsList = new List<BuildTargetSettings>();

            IList<IBuildTargetDetector> buildTargetDetectors = languageDetector.GetBuildTargetDetectors();
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

        private static IList<DeployTargetSettings> GetDeployTargetSettingsList(ILanguageDetector languageDetector, TreeAnalysis treeAnalysis)
        {
            List<DeployTargetSettings> deployTargetSettingsList = new List<DeployTargetSettings>();

            IList<IDeployTargetDetector> deployTargetDetectors = languageDetector.GetDeployTargetDetectors();
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

        private static List<ILanguageDetector> GetLanguageDetectors()
        {
            var type = typeof(LanguageDetectorBase);
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => type.IsAssignableFrom(p) && !p.IsAbstract && !p.IsInterface);
            List<ILanguageDetector> languageDetectors = new List<ILanguageDetector>();

            foreach (var langType in types)
            {
                languageDetectors.Add((ILanguageDetector)Activator.CreateInstance(langType));
            }

            return languageDetectors;
        }
    }
}
