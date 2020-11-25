namespace repo_analysis_detectors.tests.project.Models
{
    using System.Collections.Generic;
    public class ApplicationSettings
    {
        public string Language { get; set; }
        public string BuildTargetName { get; set; }
        public string DeployTargetName { get; set; }
        public Dictionary<string, object> Settings { get; set; }
    }
}
