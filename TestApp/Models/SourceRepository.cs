namespace TestApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class SourceRepository
    {
        [DataMember(IsRequired = false)]
        public string WorkingDirectory { get; set; }

        [DataMember]
        public CodeRepository Repository { get; set; }
    }

    [DataContract]
    public class CodeRepository
    {
        public CodeRepository()
        {
            this.Properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets or sets the type of repository.
        /// </summary>
        [DataMember]
        [Required]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the repository.
        /// </summary>
        [DataMember]
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the friendly name of the repository.
        /// </summary>
        [DataMember]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URL of the repository.
        /// </summary>
        [DataMember]
        public Uri Url
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default branch of the repository.
        /// </summary>
        [DataMember]
        public string DefaultBranch
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the authorization details used to access the repository.
        /// </summary>
        [DataMember]
        public Authorization AuthorizationInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the repository specific properties.
        /// </summary>
        [DataMember]
        public IDictionary<string, string> Properties { get; private set; }
    }

    [DataContract]
    public class Authorization
    {
        public Authorization()
        {
            this.Parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets or sets the authorization scheme.
        /// </summary>
        [DataMember]
        [Required]
        public string Scheme
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the authorization parameters.
        /// </summary>
        [DataMember]
        [Required]
        public IDictionary<string, string> Parameters { get; set; }
    }
}