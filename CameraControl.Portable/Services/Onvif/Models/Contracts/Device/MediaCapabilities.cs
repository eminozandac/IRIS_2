using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Device
{
    [XmlType("MediaCapabilities", Namespace = Namespaces.OnvifSchema)]
    public class MediaCapabilities
    {
        [XmlElement(DataType = "anyURI", Order = 0)]
        public string XAddr { get; set; }

        [XmlElement("StreamingCapabilities", Order = 1)]
        public RealTimeStreamingCapabilities StreamingCapabilities { get; set; }

        [XmlElement("Extension", Order = 2)]
        public MediaCapabilitiesExtension Extension { get; set; }
    }
}
