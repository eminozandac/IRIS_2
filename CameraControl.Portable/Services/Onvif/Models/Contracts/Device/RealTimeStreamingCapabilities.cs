using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Device
{
    [XmlType("RealTimeStreamingCapabilities", Namespace = Namespaces.OnvifSchema)]
    public class RealTimeStreamingCapabilities
    {
        [XmlElement("RTPMulticast", Order = 0)]
        public bool RTPMulticast { get; set; }

        [XmlElement("RTP_TCP", Order = 1)]
        public bool RTP_TCP { get; set; }

        [XmlElement("RTP_RTSP_TCP", Order = 2)]
        public bool RTP_RTSP_TCP { get; set; }

        [XmlElement("Extension", Order = 3)]
        public RealTimeStreamingCapabilitiesExtension Extension { get; set; }
    }
}
