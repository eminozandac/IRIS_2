using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.PTZ
{
    [XmlRoot("Stop", Namespace = Namespaces.PTZManagement)]
    public class Stop
    {
        [XmlElement("ProfileToken", Order = 0)]
        public string ProfileToken { get; set; }

        [XmlElement("PanTilt", Order = 1)]
        public bool PanTilt { get; set; }

        [XmlElement("Zoom", Order = 2)]
        public bool Zoom { get; set; }
    }
}
