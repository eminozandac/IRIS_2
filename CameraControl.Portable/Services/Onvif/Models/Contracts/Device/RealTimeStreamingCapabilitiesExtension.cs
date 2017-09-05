using System.Xml.Linq;
using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Device
{
    [XmlType("RealTimeStreamingCapabilitiesExtension", Namespace = Namespaces.OnvifSchema)]
    public class RealTimeStreamingCapabilitiesExtension
    {
        public XElement[] Any { get; set; }
    }
}
