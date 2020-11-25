namespace repo_analysis_detectors.tests.project
{
    using repo_analysis_detectors.tests.project.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GitHub.Services.RepositoryAnalysis.Detectors.Models;
    using GitHub.Services.RepositoryAnalysis.Detectors.LanguageDetectors;
    using System;
    using System.Linq;
    using GitHub.Services.RepositoryAnalysis.Detectors.BuildTargetDetectors;
    using GitHub.Services.RepositoryAnalysis.Detectors.DeployTargetDetectors;

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

            StartE2EFlow(treeAnalysis);
        }

        private static List<ApplicationSettings> StartE2EFlow(TreeAnalysis treeAnalysis)
        {
            List<ApplicationSettings> applicationSettingsList = new List<ApplicationSettings>();
            IReadOnlyList<ILanguageDetector> languageDetectors = GetLanguageDetectors(); // will be empty, as there are none

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

        private static List<ApplicationSettings> GetApplicationSettings(string language, IList<BuildTargetSettings> buildTargetSettingsList, IList<DeployTargetSettings> deployTargetSettingsList)
        {
            throw new NotImplementedException();
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
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => type.IsAssignableFrom(p));
            List<ILanguageDetector> languageDetectors = new List<ILanguageDetector>();

            foreach (var langType in types)
            {
                languageDetectors.Add((ILanguageDetector)Activator.CreateInstance(langType));
            }

            return languageDetectors;
        }
    }
}
