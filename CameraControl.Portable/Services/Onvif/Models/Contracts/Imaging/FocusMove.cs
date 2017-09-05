using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Imaging
{
    [XmlType("FocusMove", Namespace = Namespaces.OnvifSchema)]
    public class FocusMove
    {
        [XmlElement("Absolute", Order = 0)]
        public AbsoluteFocus Absolute { get; set; }

        [XmlElement("Relative", Order = 1)]
        public RelativeFocus Relative { get; set; }

        [XmlElement("Continuous", Order = 2)]
        public ContinuousFocus Continuous { get; set; }
    }
}
