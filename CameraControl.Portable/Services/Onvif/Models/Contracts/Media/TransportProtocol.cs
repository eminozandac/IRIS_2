using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Media
{
    [XmlType("TransportProtocol", Namespace = Namespaces.OnvifSchema)]
    public enum TransportProtocol
    {
        UDP,
        TCP,
        RTSP,
        HTTP
    }
}
