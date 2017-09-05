using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Media
{
    [XmlType("VideoSourceConfiguration", Namespace = Namespaces.OnvifSchema)]
    public class VideoSourceConfiguration
    {
        [XmlElement("SourceToken", Order = 0)]
        public string SourceToken { get; set; }

        //[XmlElement("Bounds", Order = 1)]
        //public IntRectangle Bounds { get; set; }

        //[XmlElement("Extension", Order = 3)]
        //public VideoSourceConfigurationExtension Extension { get; set; }
    }
}
