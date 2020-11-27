namespace GitHub.RepositoryAnalysis.Detectors.LanguageDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using GitHub.RepositoryAnalysis.Detectors.Utilities;
    using System.Collections.Generic;

    public class DotNetLanguageDetector : DotNetLanguageDetectorBase
    {
        public override string Language => Constants.DotNetFramework;
        public override bool IsLanguageDotNet(List<string> filesReadNames, TreeAnalysis treeAnalysis)
        {
            if (base.IsLanguageDotNet(filesReadNames, treeAnalysis))
                return true;
            foreach (var csprojFileName in filesReadNames)
            {
                if (!string.IsNullOrEmpty(CsprojParser.GetNetFxVersion(treeAnalysis.FilesContent[csprojFileName].ToXMLDoc())))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
