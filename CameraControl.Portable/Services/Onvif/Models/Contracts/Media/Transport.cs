using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Media
{
    [XmlType("Transport", Namespace = Namespaces.OnvifSchema)]
    public class Transport
    {
        [XmlElement("Protocol", Order = 0)]
        public TransportProtocol Protocol { get; set; }

        [XmlElement("Tunnel", Order = 1)]
        public Transport Tunnel { get; set; }
    }
}
