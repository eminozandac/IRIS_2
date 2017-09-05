using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Media
{
    [XmlType("MediaUri", Namespace = Namespaces.OnvifSchema)]
    public class MediaUri
    {
        [XmlElement("Uri", DataType = "anyURI", Order = 0)]
        public string Uri { get; set; }

        [XmlElement("InvalidAfterConnect", Order = 1)]
        public bool InvalidAfterConnect { get; set; }

        [XmlElement("InvalidAfterReboot", Order = 2)]
        public bool InvalidAfterReboot { get; set; }

        [XmlElement("Timeout", DataType = "duration", Order = 3)]
        public string Timeout { get; set; }
    }
}
