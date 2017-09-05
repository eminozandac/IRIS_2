using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Imaging
{
    [XmlType("AutoFocusMode", Namespace = Namespaces.OnvifSchema)]
    public enum AutoFocusMode
    {
        AUTO,
        MANUAL
    }
}
