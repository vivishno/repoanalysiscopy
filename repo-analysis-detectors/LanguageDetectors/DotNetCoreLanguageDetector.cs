namespace GitHub.RepositoryAnalysis.Detectors.LanguageDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;

    public class DotNetCoreLanguageDetector : DotNetLanguageDetectorBase
    {
        public override string Language => Constants.DotNetCore;
    }
}
