using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Imaging
{
    [XmlType("AbsoluteFocus", Namespace = Namespaces.OnvifSchema)]
    public class AbsoluteFocus
    {
        [XmlElement("Position", Order = 0)]
        public float Position { get; set; }
        
        [XmlElement("Speed", Order = 1)]
        public float? Speed { get; set; }
    }
}
