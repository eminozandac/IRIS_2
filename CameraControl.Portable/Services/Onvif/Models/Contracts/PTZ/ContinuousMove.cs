using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.PTZ
{
    [XmlRoot("ContinuousMove", Namespace = Namespaces.PTZManagement)]
    public class ContinuousMove
    {
        [XmlElement("ProfileToken", Order = 0)]
        public string ProfileToken { get; set; }

        [XmlElement("Velocity", Order = 1)]
        public PTZSpeed Velocity { get; set; }
    }
}
