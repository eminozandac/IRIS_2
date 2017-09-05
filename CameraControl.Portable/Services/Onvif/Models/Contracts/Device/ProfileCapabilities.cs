using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Device
{
    [XmlType("ProfileCapabilities", Namespace = Namespaces.OnvifSchema)]
    public class ProfileCapabilities
    {
        [XmlElement(Order = 0)]
        public int MaximumNumberOfProfiles { get; set; }
    }
}
