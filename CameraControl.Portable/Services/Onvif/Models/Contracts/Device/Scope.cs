using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Device
{
    [XmlType("Scope", Namespace = Namespaces.OnvifSchema)]
    public class Scope
    {
        [XmlElement("ScopeDef", Order = 0)]
        public ScopeDefinition ScopeDef { get; set; }

        [XmlElement("ScopeItem", DataType = "anyURI", Order = 1)]
        public string ScopeItem { get; set; }
    }
}
