using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Device
{
    [XmlType("MediaCapabilitiesExtension", Namespace = Namespaces.OnvifSchema)]
    public class MediaCapabilitiesExtension
    {
        [XmlElement("ProfileCapabilities", Order = 0)]
        public ProfileCapabilities ProfileCapabilities { get; set; }
    }
}
