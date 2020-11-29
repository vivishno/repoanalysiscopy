namespace TestApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GitHub.RepositoryAnalysis.Detectors.LanguageDetectors;
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using Microsoft.VisualStudio.Services.PortalExtension.RepositoryAnalysis.WebApi.Contracts;
    using SourceRepository = Models.SourceRepository;

    public class RepositoryAnalysisService
    {
        public async Task<RepositoryAnalysis> Analyze(SourceRepository sourceRepository)
        {
            TreeAnalysisService treeAnalysisService = new TreeAnalysisService();

            var treeAnalysis = await treeAnalysisService.GetTreeAnalysis(sourceRepository);

            return StartE2EFlow(treeAnalysis);
        }

        private RepositoryAnalysis StartE2EFlow(TreeAnalysis treeAnalysis)
        {
            List<ApplicationSettings> applicationSettingsList = new List<ApplicationSettings>();
            IReadOnlyList<ILanguageDetector> languageDetectors = GetLanguageDetectors();

            foreach (var detector in languageDetectors)
            {
                if (!detector.IsLanguageDetected(treeAnalysis))
                {
                    continue;
                }

                List<ApplicationSettings> detectorSettingsList = (List<ApplicationSettings>)detector.GetApplicationSettings(treeAnalysis);
                applicationSettingsList.AddRange(detectorSettingsList);
            }

            return new RepositoryAnalysis
            {
                ApplicationSettingsList = applicationSettingsList
            };
        }

        private List<ILanguageDetector> GetLanguageDetectors()
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
