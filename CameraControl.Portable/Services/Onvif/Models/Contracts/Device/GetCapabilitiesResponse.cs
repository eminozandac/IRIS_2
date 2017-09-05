using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Device
{
    [XmlRoot("GetCapabilitiesResponse", Namespace = Namespaces.DeviceManagement)]
    public class GetCapabilitiesResponse
    {
        [XmlElement("Capabilities")]
        public Capabilities Capabilities { get; set; }
    }
}
