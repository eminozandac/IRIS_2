using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Device
{
    [XmlType("Capabilities", Namespace = Namespaces.OnvifSchema)]
    public class Capabilities
    {
        //[XmlElement("Analytics", Order = 0)]
        //public AnalyticsCapabilities Analytics { get; set; }

        //[XmlElement("Device", Order = 1)]
        //public DeviceCapabilities Device { get; set; }

        //[XmlElement("Events", Order = 2)]
        //public EventCapabilities Events { get; set; }

        //[XmlElement("Imaging", Order = 3)]
        //public ImagingCapabilities Imaging { get; set; }

        [XmlElement("Media", Order = 4)]
        public MediaCapabilities Media { get; set; }

        //[XmlElement("PTZ", Order = 5)]
        //public PTZCapabilities PTZ { get; set; }

        //[XmlElement("Extension", Order = 6)]
        //public CapabilitiesExtension Extension { get; set; }
    }
}
