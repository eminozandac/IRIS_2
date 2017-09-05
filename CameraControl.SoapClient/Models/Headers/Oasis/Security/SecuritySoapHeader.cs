using System.Xml.Serialization;

namespace CameraControl.SoapClient.Models.Headers.Oasis.Security
{
    /// <summary>
    ///     Represents the SOAP Security Header
    /// </summary>
    [XmlRoot("Security", Namespace = Constants.OrgOpenOasisDocsWss200401Oasis200401WssWssecuritySecext10)]
    public class SecuritySoapHeader : SoapHeader
    {
        /// <summary>
        ///     Creates a new instance
        /// </summary>
        public SecuritySoapHeader()
        {
            MustUnderstand = 1;
        }

        /// <summary>
        ///     The header timestamp
        /// </summary>
        [XmlElement("Timestamp", Namespace = Constants.OrgOpenOasisDocsWss200401Oasis200401WssWssecurityUtility10)]
        public Timestamp Timestamp { get; set; }
    }
}
