using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Media
{
    [XmlRoot("GetStreamUriResponse", Namespace = Namespaces.MediaManagement)]
    public class GetStreamUriResponse
    {
        [XmlElement("MediaUri")]
        public MediaUri MediaUri { get; set; }
    }
}
