// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsprojParser.cs" company="Microsoft Corporation">
//   2012-2023, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace GitHub.RepositoryAnalysis.Detectors.Utilities
{
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml;

    public class CsprojParser
    {
        private static IDictionary<string, string> DotNetPrefixes;

        static CsprojParser() {
            DotNetPrefixes = new Dictionary<string, string> {
                { Constants.DotNetCorePrefix, Constants.DotNetCore },
                { Constants.DotNetStandardPrefix, Constants.DotNetCore },
                { Constants.DotNetFrameworkPrefix, Constants.DotNetFramework },
            };
        }

        internal static string GetOutputTypeValue(XmlDocument xmlDocument)
        {
            return xmlDocument.GetElementsByTagName(Constants.OutputTypeTag)?.Item(0)?.FirstChild?.Value ?? string.Empty;
        }

        internal static KeyValuePair<string, string> GetDotNetVersion(XmlDocument xmlDocument)
        {
            KeyValuePair<string, string> result = new KeyValuePair<string, string>(string.Empty, string.Empty);
            if (xmlDocument != null && xmlDocument.HasChildNodes)
            {
                string dotNetVersion = xmlDocument.GetElementsByTagName(Constants.TargetFrameworksTag)?.Item(0)?.FirstChild?.Value ?? xmlDocument.GetElementsByTagName(Constants.TargetFrameworkTag)?.Item(0)?.FirstChild?.Value;
                result = GetTargetVersion(dotNetVersion ?? string.Empty);
            }
            return result;
        }

        internal static string GetNetFxVersion(XmlDocument xmlDocument)
        {
            var dotNetVersion = GetDotNetVersion(xmlDocument);
            if (Constants.DotNetFramework.Equals(dotNetVersion.Key))
            {
                return dotNetVersion.Value;
            }
            return xmlDocument.GetElementsByTagName(Constants.TargetFrameworkVersionTag)?.Item(0)?.FirstChild?.Value ?? string.Empty;
        }

        internal static bool HasElement(XmlDocument xmlDocument, string requiredTag)
        {
            if (xmlDocument != null && xmlDocument.HasChildNodes)
            {
                string tagValue = xmlDocument.GetElementsByTagName(requiredTag)?.Item(0)?.FirstChild?.Value;
                if (!string.IsNullOrEmpty(tagValue)) return true;
            }
            return false;
        }

        internal static KeyValuePair<string, string> GetTargetVersion(string targetFrameworks)
        {
            var targetFrameworkList = targetFrameworks.Split(';');
            foreach(var targetFramework in targetFrameworkList)
            {
                foreach (var prefix in DotNetPrefixes)
                {
                    if (targetFramework.StartsWith(prefix.Key))
                        return new KeyValuePair<string, string>(prefix.Value, targetFramework);
                }
            }
            return new KeyValuePair<string, string>(string.Empty, string.Empty);
        }

        internal static string GetRuntimeFramework(XmlDocument currentProject)
        {
            var dotNetVersion = currentProject.GetElementsByTagName(Constants.TargetFrameworksTag)?.Item(0)?.FirstChild?.Value ?? currentProject.GetElementsByTagName(Constants.TargetFrameworkTag)?.Item(0)?.FirstChild?.Value;
            return GetTargetVersion(dotNetVersion ?? string.Empty).Value;
        }

        internal static string GetSdkVersion(string globalJsonContents)
        {
            GlobalJsonModel globalJson = JsonConvert.DeserializeObject<GlobalJsonModel>(globalJsonContents);
            return globalJson.sdk?.version ?? string.Empty;
        }

        internal static string GetSdkValue(XmlDocument xmlDocument)
        {
            return xmlDocument.DocumentElement?.GetAttribute(Constants.SdkTag) ?? string.Empty;
        }
    }

    public class CsprojMetadata
    {
        public string ProjectName;
        public string TargetFramework;
        public string DefaultSdkVersion;
    }
}
