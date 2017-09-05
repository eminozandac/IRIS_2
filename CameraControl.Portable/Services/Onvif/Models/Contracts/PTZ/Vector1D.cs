using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.PTZ
{
    [XmlType(Namespace = Namespaces.OnvifSchema)]
    public class Vector1D
    {
        [XmlAttribute]
        public float x { get; set; }
    }
}
