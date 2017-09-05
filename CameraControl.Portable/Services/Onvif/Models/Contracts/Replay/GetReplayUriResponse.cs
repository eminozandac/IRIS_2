using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Replay
{
    [XmlRoot("GetReplayUriResponse", Namespace = Namespaces.ReplayManagement)]
    public class GetReplayUriResponse
    {
        [XmlElement("Uri")]
        public string Uri { get; set; }
    }
}
