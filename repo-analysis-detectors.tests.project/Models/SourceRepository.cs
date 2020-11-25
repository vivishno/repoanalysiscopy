namespace repo_analysis_detectors.tests.project.Models
{
    using System;
    using System.Collections.Generic;
    public class SourceRepository
    {
        public string WorkingDirectory { get; set; }
        public CodeRepository Repository { get; set; }
    }

    public class CodeRepository
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public Uri Url { get; set; }
        public string DefaultBranch { get; set; }
        public Authorization AuthorizationInfo { get; set; }
        public IDictionary<string, string> Properties { get; }
    }

    public class Authorization
    {
        public string Scheme { get; set; }
        public IDictionary<string, string> Parameters { get; set; }
    }
}
