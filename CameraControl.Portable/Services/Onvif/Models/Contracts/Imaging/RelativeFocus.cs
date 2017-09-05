using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Imaging
{
    [XmlType("RelativeFocus", Namespace = Namespaces.OnvifSchema)]
    public class RelativeFocus
    {
        [XmlElement("Distance", Order = 0)]
        public float Distance { get; set; }
        
        [XmlElement("Speed", Order = 1)]
        public float? Speed { get; set; }
    }
}
