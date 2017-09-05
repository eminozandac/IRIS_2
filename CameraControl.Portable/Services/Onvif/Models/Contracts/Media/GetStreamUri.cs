using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Media
{
    [XmlRoot("GetStreamUri", Namespace = Namespaces.MediaManagement)]
    public class GetStreamUri
    {
        [XmlElement("StreamSetup", Order = 0)]
        public StreamSetup StreamSetup { get; set; }

        [XmlElement("ProfileToken", Order = 1)]
        public string ProfileToken { get; set; }
    }
}
