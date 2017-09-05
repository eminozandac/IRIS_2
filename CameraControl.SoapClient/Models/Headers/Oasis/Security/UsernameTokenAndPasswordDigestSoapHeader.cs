using System.Xml.Serialization;

namespace CameraControl.SoapClient.Models.Headers.Oasis.Security
{
    /// <summary>
    ///     The SOAP Security header for UsernameToken Profile with digested password
    /// </summary>
    [XmlRoot("Security", Namespace = Constants.OrgOpenOasisDocsWss200401Oasis200401WssWssecuritySecext10)]
    public class UsernameTokenAndPasswordDigestSoapHeader : SecuritySoapHeader
    {
        /// <summary>
        ///     Creates a new instance
        /// </summary>
        public UsernameTokenAndPasswordDigestSoapHeader()
        {
            MustUnderstand = 1;
        }

        /// <summary>
        ///     The username token
        /// </summary>
        [XmlElement("UsernameToken")]
        public UsernameTokenWithPasswordDigest UsernameToken { get; set; }
    }
}
