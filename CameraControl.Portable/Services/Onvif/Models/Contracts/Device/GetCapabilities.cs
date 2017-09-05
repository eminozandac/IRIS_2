using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Device
{
    [XmlRoot("GetCapabilities", Namespace = Namespaces.DeviceManagement)]
    public class GetCapabilities
    {
        [XmlElement("Category", Order = 0)]
        public CapabilityCategory[] Category { get; set; }
    }
}
