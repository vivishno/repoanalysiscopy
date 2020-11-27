namespace GitHub.RepositoryAnalysis.Detectors.LanguageDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;

    public class PythonLanguageDetector : LanguageDetectorBase
    {
        public override string Language => Constants.PythonLanguageName;

        public override bool IsLanguageDetected(TreeAnalysis treeAnalysis)
        {
            if (treeAnalysis.FileExtensions.Contains(Constants.PythonFileExtension))
            {
                return true;
            }
            return false;
        }
    }
}