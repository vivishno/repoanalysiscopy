namespace GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using GitHub.RepositoryAnalysis.Detectors.Utilities;
    using System.Xml;

    public class DotNetCoreWorkerBuildTargetDetector : DotNetBuildTargetDetectorBase
    {
        public override string BuildTargetName => Constants.DotNetCoreWorkerBuildTargetName;

        internal override bool ConfirmIfCriteriaMatched(XmlDocument xmlDocument)
        {
            return base.ConfirmIfCriteriaMatched(xmlDocument) && Constants.DotNetCoreWorkerSdkName.Equals(CsprojParser.GetSdkValue(xmlDocument));
        }
    }
}
