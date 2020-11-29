// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DotNetCoreConsoleBuildTargetDetector.cs" company="Microsoft Corporation">
//   2012-2023, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using GitHub.RepositoryAnalysis.Detectors.Utilities;
    using System.Xml;

    public class DotNetCoreConsoleBuildTargetDetector : DotNetBuildTargetDetectorBase
    {
        public override string BuildTargetName => Constants.DotNetCoreConsoleBuildTargetName;

        internal override bool ConfirmIfCriteriaMatched(XmlDocument xmlDocument)
        {
            return base.ConfirmIfCriteriaMatched(xmlDocument) && Constants.DotNetCoreConsoleOutputType.Equals(CsprojParser.GetOutputTypeValue(xmlDocument));
        }
    }
}
