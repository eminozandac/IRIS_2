using System.Xml.Serialization;

namespace CameraControl.SoapClient.Models.Headers.Oasis.Security
{
    /// <summary>
    ///     Represents an username token with digested password
    /// </summary>
    [XmlType("UsernameToken")]
    public class UsernameTokenWithPasswordDigest
    {
        /// <summary>
        ///     The username value
        /// </summary>
        [XmlElement("Username")]
        public string Username { get; set; }

        /// <summary>
        ///     The digested password element
        /// </summary>
        [XmlElement("Password")]
        public UsernameTokenPasswordDigest Password { get; set; }

        /// <summary>
        ///     The nonce element
        /// </summary>
        [XmlElement("Nonce")]
        public UsernameTokenNonce Nonce { get; set; }

        /// <summary>
        ///     The created date string element
        /// </summary>
        [XmlElement("Created", Namespace = Constants.OrgOpenOasisDocsWss200401Oasis200401WssWssecurityUtility10)]
        public string Created { get; set; }
    }
}
