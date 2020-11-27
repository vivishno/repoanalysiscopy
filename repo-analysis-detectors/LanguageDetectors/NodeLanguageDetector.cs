namespace GitHub.RepositoryAnalysis.Detectors.LanguageDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;

    public class NodeLanguageDetector : LanguageDetectorBase
    {
        public override string Language => Constants.NodeLanguageName;

        public override bool IsLanguageDetected(TreeAnalysis treeAnalysis)
        {
            if (treeAnalysis.FilesInfo.ContainsKey(Constants.PackageJsonFileName))
            {
                return true;
            }
            return false;
        }
    }
}