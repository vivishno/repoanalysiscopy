namespace GitHub.RepositoryAnalysis.Detectors.LanguageDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using GitHub.RepositoryAnalysis.Detectors.Utilities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public abstract class DotNetLanguageDetectorBase: LanguageDetectorBase
    {
        public override bool IsLanguageDetected(TreeAnalysis treeAnalysis)
        {
            var filesReadNames = new List<string>(treeAnalysis.FilesContent.Keys.Where(x => Regex.IsMatch(x, Constants.CsProjDetectionRegex, RegexOptions.IgnoreCase)));
            return IsLanguageDotNet(filesReadNames, treeAnalysis);
        }

        public virtual bool IsLanguageDotNet(List<string> filesReadNames, TreeAnalysis treeAnalysis)
        {
            foreach(var csprojFileName in filesReadNames)
            {
                if (Language.Equals(CsprojParser.GetDotNetVersion(treeAnalysis.FilesContent[csprojFileName].ToXMLDoc()).Key))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
