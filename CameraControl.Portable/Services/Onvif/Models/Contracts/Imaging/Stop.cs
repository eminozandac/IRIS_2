using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Imaging
{
    [XmlRoot("Stop", Namespace = Namespaces.ImagingManagement)]
    public class Stop
    {
        [XmlElement("VideoSourceToken", Order = 0)]
        public string VideoSourceToken { get; set; }
    }
}
