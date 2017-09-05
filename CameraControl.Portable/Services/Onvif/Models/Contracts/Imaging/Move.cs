using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Imaging
{
    [XmlRoot("Move", Namespace = Namespaces.ImagingManagement)]
    public class Move
    {
        [XmlElement("VideoSourceToken", Order = 0)]
        public string VideoSourceToken { get; set; }

        [XmlElement("Focus", Order = 1)]
        public FocusMove Focus { get; set; }
    }
}
