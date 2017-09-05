using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Media
{
    [XmlType("StreamType", Namespace = Namespaces.OnvifSchema)]
    public enum StreamType
    {
        [XmlEnum("RTP-Unicast")] RTPUnicast,
        [XmlEnum("RTP-Multicast")] RTPMulticast
    }
}
