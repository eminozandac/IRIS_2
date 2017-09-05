using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Device
{
    [XmlType("ScopeDefinition", Namespace = Namespaces.OnvifSchema)]
    public enum ScopeDefinition
    {
        Fixed,
        Configurable
    }
}
