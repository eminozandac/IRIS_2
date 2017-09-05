using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.PTZ
{
    [XmlRoot("GotoHomePositionResponse", Namespace = Namespaces.PTZManagement)]
    public class GotoHomePositionResponse
    {
    }
}
