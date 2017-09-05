using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Device
{
    [XmlRoot("GetScopesResponse", Namespace = Namespaces.DeviceManagement)]
    public class GetScopesResponse
    {
        [XmlElement("Scopes")]
        public Scope[] Scopes { get; set; }
    }
}
