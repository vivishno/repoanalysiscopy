namespace GitHub.RepositoryAnalysis.Detectors.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class RepositoryAnalysis
    {
        [DataMember]
        public List<ApplicationSettings> ApplicationSettingsList { get; set; }
    }
}
