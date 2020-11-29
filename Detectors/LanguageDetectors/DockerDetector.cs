namespace GitHub.RepositoryAnalysis.Detectors.LanguageDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;

    public class DockerDetector : LanguageDetectorBase
    {
        public override string Language => Constants.Docker;

        public override bool IsLanguageDetected(TreeAnalysis treeAnalysis)
        {
            if (treeAnalysis.FilesInfo.ContainsKey(Constants.Dockerfile))
            {
                return true;
            }
            return false;
        }
    }
}
