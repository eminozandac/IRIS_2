using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Replay
{
    [XmlRoot("GetReplayUri", Namespace = Namespaces.ReplayManagement)]
    public class GetReplayUri
    {
        [XmlElement("StreamSetup", Order = 0)]
        public StreamSetup StreamSetup { get; set; }

        [XmlElement("RecordingToken", Order = 1)]
        public string RecordingToken { get; set; }
    }
}
