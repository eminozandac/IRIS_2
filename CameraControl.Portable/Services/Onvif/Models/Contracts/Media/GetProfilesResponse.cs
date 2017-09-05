using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Media
{
    [XmlRoot("GetProfilesResponse", Namespace = Namespaces.MediaManagement)]
    public class GetProfilesResponse
    {
        [XmlElement("Profiles")]
        public Profile[] Profiles { get; set; }
    }
}
