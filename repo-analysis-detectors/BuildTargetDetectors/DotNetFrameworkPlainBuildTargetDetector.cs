namespace GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;

    public class DotNetFrameworkPlainBuildTargetDetector: DotNetFrameworkBuildTargetDetectorBase
    {
        public override string BuildTargetName => Constants.DotNetFrameworkPlainBuildTarget;
    }
}
