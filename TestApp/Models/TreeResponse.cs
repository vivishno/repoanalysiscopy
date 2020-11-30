namespace TestApp.Models
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class TreeResponse
    {
        [JsonPropertyName("sha")]
        public string SHA { get; set; }
        
        [JsonPropertyName("url")]
        public string Url { get; set; }
        
        [JsonPropertyName("truncated")]
        public bool Truncated { get; set; }
        
        [JsonPropertyName("tree")]
        public IList<GitTreeNode> Tree { get; set; }
    }

    public class GitTreeNode
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("mode")]
        public string Mode { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("sha")]
        public string SHA { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }
    }
}
