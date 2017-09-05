using System.Xml.Serialization;

namespace CameraControl.SoapClient.Models.Headers.Oasis.Security
{
    /// <summary>
    ///     Represents nonce text
    /// </summary>
    [XmlType("Nonce")]
    public class UsernameTokenNonce
    {
        /// <summary>
        ///     The nonce encoding type
        /// </summary>
        [XmlAttribute("EncodingType")]
        public string EncodingType { get; set; }

        /// <summary>
        ///     The nonce value
        /// </summary>
        [XmlText]
        public string Value { get; set; }

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        public UsernameTokenNonce()
        {
            EncodingType = Constants.OrgOpenOasisDocsWss200401Oasis200401WssUsernameTokenProfile10Nonce;
        }
    }
}
