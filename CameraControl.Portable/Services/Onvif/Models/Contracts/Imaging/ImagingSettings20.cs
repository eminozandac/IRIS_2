using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Imaging
{
    [XmlType("ImagingSettings20", Namespace = Namespaces.OnvifSchema)]
    public class ImagingSettings20
    {
        //[XmlElement("BacklightCompensation", Order = 0)]
        //public BacklightCompensation20 BacklightCompensation { get; set; }

        [XmlElement("Brightness", Order = 1)]
        public float? Brightness { get; set; }

        [XmlElement("ColorSaturation", Order = 2)]
        public float? ColorSaturation { get; set; }

        [XmlElement("Contrast", Order = 3)]
        public float? Contrast { get; set; }

        //[XmlElement("Exposure", Order = 4)]
        //public Exposure20 Exposure { get; set; }

        [XmlElement("Focus", Order = 5)]
        public FocusConfiguration20 Focus { get; set; }

        //[XmlElement("IrCutFilter", Order = 6)]
        //public IrCutFilterMode IrCutFilter { get; set; }

        [XmlElement("Sharpness", Order = 7)]
        public float? Sharpness { get; set; }

        //[XmlElement("WideDynamicRange", Order = 8)]
        //public WideDynamicRange20 WideDynamicRange { get; set; }

        //[XmlElement("WhiteBalance", Order = 9)]
        //public WhiteBalance20 WhiteBalance { get; set; }

        //[XmlElement("Extension", Order = 10)]
        //public ImagingSettingsExtension20 Extension { get; set; }
    }
}
