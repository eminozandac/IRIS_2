using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Device
{
    [XmlRoot("GetScopes", Namespace = Namespaces.DeviceManagement)]
    public class GetScopes
    {
    }
}
