using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Imaging
{
    [XmlRoot("SetImagingSettings", Namespace = Namespaces.ImagingManagement)]
    public class SetImagingSettings
    {
        [XmlElement("VideoSourceToken", Order = 0)]
        public string VideoSourceToken { get; set; }

        [XmlElement("ImagingSettings", Order = 1)]
        public ImagingSettings20 ImagingSettings { get; set; }

        [XmlElement("ForcePersistence", Order = 2)]
        public bool ForcePersistence { get; set; }
    }
}
