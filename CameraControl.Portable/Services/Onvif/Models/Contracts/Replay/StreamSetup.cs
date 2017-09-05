using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Replay
{
    [XmlType("StreamSetup", Namespace = Namespaces.OnvifSchema)]
    public class StreamSetup
    {
        [XmlElement("StreamType", Order = 0)]
        public StreamType Stream { get; set; }

        [XmlElement("Transport", Order = 1)]
        public Transport Transport { get; set; }
    }
}
