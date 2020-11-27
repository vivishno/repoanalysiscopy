namespace GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using GitHub.RepositoryAnalysis.Detectors.Utilities;
    using System.Xml;

    public class DotNetFrameworkLibraryBuildTargetDetector: DotNetFrameworkBuildTargetDetectorBase
    {
        public override string BuildTargetName => Constants.DotNetFrameworkLibraryBuildTarget;

        public override bool IsBuildTargetMatched(XmlDocument currentCsproj)
        {
            if (base.IsBuildTargetMatched(currentCsproj))
                return Constants.DotNetFrameworkLibraryOutputType.Equals(CsprojParser.GetOutputTypeValue(currentCsproj));
            return false;
        }
    }
}
