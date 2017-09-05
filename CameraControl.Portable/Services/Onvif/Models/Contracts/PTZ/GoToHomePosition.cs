using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.PTZ
{
    [XmlRoot("GotoHomePosition", Namespace = Namespaces.PTZManagement)]
    public class GotoHomePosition
    {
        [XmlElement("ProfileToken", Order = 0)]
        public string ProfileToken { get; set; }

        [XmlElement("Speed", Order = 1)]
        public PTZSpeed Speed { get; set; }
    }
}
