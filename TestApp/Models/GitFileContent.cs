namespace TestApp.Models
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
