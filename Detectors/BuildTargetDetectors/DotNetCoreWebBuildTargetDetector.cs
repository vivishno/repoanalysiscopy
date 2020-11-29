namespace GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using GitHub.RepositoryAnalysis.Detectors.Utilities;
    using System.Xml;

    public class DotNetCoreWebBuildTargetDetector : DotNetBuildTargetDetectorBase
    {
        public override string BuildTargetName => Constants.DotNetCoreWebBuildTargetName;

        internal override bool ConfirmIfCriteriaMatched(XmlDocument xmlDocument)
        {
            return base.ConfirmIfCriteriaMatched(xmlDocument) && Constants.DotNetCoreWebSdkName.Equals(CsprojParser.GetSdkValue(xmlDocument));
        }
    }
}
