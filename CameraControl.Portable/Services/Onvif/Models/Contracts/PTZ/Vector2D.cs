using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.PTZ
{
    [XmlType(Namespace = Namespaces.OnvifSchema)]
    public class Vector2D
    {
        [XmlAttribute]
        public float x { get; set; }
        
        [XmlAttribute]
        public float y { get; set; }
    }
}
