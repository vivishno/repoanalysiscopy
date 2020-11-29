namespace GitHub.RepositoryAnalysis.Detectors.BuildTargetDetectors
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using GitHub.RepositoryAnalysis.Detectors.Utilities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml;

    public abstract class DotNetBuildTargetDetectorBase : BuildTargetDetectorBase
    {
        public List<CsprojMetadata> QualifiedProjects;

        public DotNetBuildTargetDetectorBase()
        {
            QualifiedProjects = new List<CsprojMetadata>();
        }

        public override List<BuildTargetSettings> GetBuildTargetSettings(TreeAnalysis treeAnalysis)
        {
            List<BuildTargetSettings> buildTargetSettingsList = new List<BuildTargetSettings>();

            foreach (var qualifiedProject in QualifiedProjects)
            {
                var node = treeAnalysis.FilesInfo[qualifiedProject.ProjectName.Split('/').LastOrDefault()].First();
                var directoryPath = node.GetDirectoryPath();
                string sdkVersion = qualifiedProject.DefaultSdkVersion;
                if(treeAnalysis.FilesInfo.ContainsKey(Constants.GlobalJsonFileName))
                {
                    foreach (var globalJsonNode in treeAnalysis.FilesInfo[Constants.GlobalJsonFileName])
                    {
                        if (globalJsonNode.GetDirectoryPath().Equals(directoryPath))
                        {
                            var sdkVersionFromGlobalJson = CsprojParser.GetSdkVersion(treeAnalysis.FilesContent[globalJsonNode.Path].Value.ToString());
                            if (!string.IsNullOrEmpty(sdkVersionFromGlobalJson)) sdkVersion = sdkVersionFromGlobalJson;
                        }
                    }
                }
                buildTargetSettingsList.Add(new BuildTargetSettings
                {
                    Name = BuildTargetName,
                    Settings = new Dictionary<string, object>
                    {
                        { Constants.WorkingDirectory, directoryPath},
                        { Constants.LanguageVersion, qualifiedProject.TargetFramework},
                        { Constants.SdkVersion, qualifiedProject.DefaultSdkVersion },
                        { Constants.CsprojPath, node.Path }
                    }
                });
            }

            return buildTargetSettingsList;
        }

        public override bool IsBuildTargetDetected(TreeAnalysis treeAnalysis)
        {
            var filesReadNames = new List<string>(treeAnalysis.FilesContent.Keys.Where(x => Regex.IsMatch(x, Constants.CsProjDetectionRegex, RegexOptions.IgnoreCase)));
            foreach (var csprojFileName in filesReadNames)
            {
                AppendIfProjectQualifies(treeAnalysis.FilesContent[csprojFileName].ToXMLDoc(), csprojFileName);
            }
            if (QualifiedProjects.Count > 0) return true;
            return false;
        }

        internal void AppendIfProjectQualifies(XmlDocument currentCsproj, string csprojFileName)
        {
            CsprojMetadata currentProjMetadata = GetMetadataIfBuildTargetMatched(currentCsproj, csprojFileName);
            if (currentProjMetadata != null)
            {
                QualifiedProjects.Add(currentProjMetadata);
            }
        }

        internal CsprojMetadata GetMetadataIfBuildTargetMatched(XmlDocument currentCsproj, string csprojFileName)
        {
            if (currentCsproj == null)
                return null;
            if (IsBuildTargetMatched(currentCsproj))
            {
                var targetFramework = CsprojParser.GetRuntimeFramework(currentCsproj);
                return new CsprojMetadata
                {
                    ProjectName = csprojFileName.Split('/').LastOrDefault(),
                    TargetFramework = targetFramework,
                    DefaultSdkVersion = GetSdkVersionFromTargetFramework(targetFramework)
                };
            }
            return null;
        }
        internal bool IsBuildTargetMatched(XmlDocument xmlDocument)
        {
            switch (BuildTargetName)
            {
                case Constants.DotNetCoreConsoleBuildTargetName:
                    return ConfirmIfCriteriaMatched(xmlDocument);
                case Constants.DotNetCoreWebBuildTargetName:
                    return ConfirmIfCriteriaMatched(xmlDocument);
                case Constants.DotNetCoreWorkerBuildTargetName:
                    return ConfirmIfCriteriaMatched(xmlDocument);
                default:
                    return false;
            }
        }

        internal virtual bool ConfirmIfCriteriaMatched(XmlDocument xmlDocument)
        {
            if (Constants.DotNetCore.Equals(CsprojParser.GetDotNetVersion(xmlDocument).Key))
            {
                return true;
            }
            return false;
        }

        internal string GetSdkVersionFromTargetFramework(string targetFramework)
        {
            if (targetFramework.Contains(Constants.DotNetCorePrefix))
                return $"{targetFramework.Replace(Constants.DotNetCorePrefix, "")}.x";
            return string.Empty;
        }
    }
}
