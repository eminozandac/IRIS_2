using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Imaging
{
    [XmlType("ContinuousFocus", Namespace = Namespaces.OnvifSchema)]
    public class ContinuousFocus
    {
        [XmlElement("Speed", Order = 0)]
        public float Speed { get; set; }
    }
}
