using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Device
{
    [XmlType("CapabilityCategory", Namespace = Namespaces.OnvifSchema)]
    public enum CapabilityCategory
    {
        All,
        Analytics,
        Device,
        Events,
        Imaging,
        Media,
        PTZ
    }
}
