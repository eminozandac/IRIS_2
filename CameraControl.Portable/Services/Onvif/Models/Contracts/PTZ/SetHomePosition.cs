using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.PTZ
{
    [XmlRoot("SetHomePosition", Namespace = Namespaces.PTZManagement)]
    public class SetHomePosition
    {
        [XmlElement("ProfileToken", Order = 0)]
        public string ProfileToken { get; set; }
    }
}
