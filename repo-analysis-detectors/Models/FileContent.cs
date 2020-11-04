namespace Microsoft.VisualStudio.PortalExtension.Server.Services.RepositoryAnalysis.Detectors.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// FileContent class
    /// Contains read value from the file, and other helper methods to cast the value into objects.
    /// </summary>
    public class FileContent
    {
        /// <summary>
        /// Whatever read from the file, stored as it is.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Converts Value into XmlDocument.
        /// </summary>
        /// <returns>Returns the Value casted to XmlDocument.</returns>
        public XmlDocument ToXMLDoc()
        {
            if (!Value.GetType().Equals(typeof(XmlDocument)))
            {
                try
                {
                    var csprojContent = new XmlDocument();
                    string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                    if (Value.ToString().StartsWith(_byteOrderMarkUtf8, StringComparison.Ordinal))
                        Value = Value.ToString().Remove(0, _byteOrderMarkUtf8.Length);
                    csprojContent.LoadXml(Value.ToString().ToLower());
                    Value = csprojContent;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return (XmlDocument)Value;
        }

        /// <summary>
        /// Deserializes "Value" to given class T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Returns Value deserialized to T</returns>
        public T ToObject<T>() where T: class
        {
            if (!Value.GetType().Equals(typeof(T)))
            {
                try
                {
                    var convertedValue = JsonConvert.DeserializeObject<T>(Value.ToString());
                    Value = convertedValue;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return (T)Value;
        }
    }
}
