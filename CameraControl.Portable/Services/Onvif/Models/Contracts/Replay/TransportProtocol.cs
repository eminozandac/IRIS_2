using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Replay
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
