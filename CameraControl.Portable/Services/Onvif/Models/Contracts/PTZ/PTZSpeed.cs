using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.PTZ
{
    [XmlType("PTZSpeed", Namespace = Namespaces.OnvifSchema)]
    public class PTZSpeed
    {
        public PTZSpeed()
        {
            PanTilt = new Vector2D();
            Zoom = new Vector1D();
        }

        [XmlElement("PanTilt", Order = 0)]
        public Vector2D PanTilt { get; set; }
        
        [XmlElement("Zoom", Order = 1)]
        public Vector1D Zoom { get; set; }
    }
}
