namespace repo_analysis_detectors.tests.project.Models
{
    using System.Text.Json.Serialization;

    public class GitFileContent
    {
        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
