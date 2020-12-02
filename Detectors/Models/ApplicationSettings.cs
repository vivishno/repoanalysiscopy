namespace GitHub.RepositoryAnalysis.Detectors.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ApplicationSettings
    {
        [DataMember]
        public string Language { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BuildTargetName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string DeployTargetName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Dictionary<string, object> Settings { get; set; }
    }
}
