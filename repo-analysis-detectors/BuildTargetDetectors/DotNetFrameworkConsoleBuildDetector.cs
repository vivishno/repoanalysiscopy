namespace GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using GitHub.RepositoryAnalysis.Detectors.Utilities;
    using System.Xml;

    public class DotNetFrameworkConsoleBuildDetector : DotNetFrameworkBuildTargetDetectorBase
    {
        public override string BuildTargetName => Constants.DotNetFrameworkConsoleBuildTarget;

        public override bool IsBuildTargetMatched(XmlDocument currentCsproj)
        {
            if (base.IsBuildTargetMatched(currentCsproj))
                return Constants.DotNetCoreConsoleOutputType.Equals(CsprojParser.GetOutputTypeValue(currentCsproj), System.StringComparison.InvariantCultureIgnoreCase);
            return false;
        }
    }
}
