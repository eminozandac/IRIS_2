using System.Xml.Serialization;

namespace CameraControl.SoapClient.Models.Headers.Oasis.Security
{
    /// <summary>
    ///     Represents digested password text
    /// </summary>
    [XmlType("Password")]
    public class UsernameTokenPasswordDigest
    {
        /// <summary>
        ///     Creates a new instance
        /// </summary>
        public UsernameTokenPasswordDigest()
        {
            Type = Constants.OrgOpenOasisDocsWss200401Oasis200401WssUsernameTokenProfile10PasswordDigest;
        }

        /// <summary>
        ///     The password type
        /// </summary>
        [XmlAttribute("Type")]
        public string Type { get; set; }

        /// <summary>
        ///     The password value
        /// </summary>
        [XmlText]
        public string Value { get; set; }
    }
}
