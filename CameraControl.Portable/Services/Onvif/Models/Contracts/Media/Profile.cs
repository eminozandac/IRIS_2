using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Media
{
    [XmlType("Profile", Namespace = Namespaces.OnvifSchema)]
    public class Profile
    {
        [XmlElement("Name", Order = 0)]
        public string Name { get; set; }

        [XmlElement("VideoSourceConfiguration", Order = 1)]
        public VideoSourceConfiguration VideoSourceConfiguration { get; set; }

        //[XmlElement("AudioSourceConfiguration", Order = 2)]
        //public AudioSourceConfiguration AudioSourceConfiguration { get; set; }

        //[XmlElement("VideoEncoderConfiguration", Order = 3)]
        //public VideoEncoderConfiguration VideoEncoderConfiguration { get; set; }

        //[XmlElement("AudioEncoderConfiguration", Order = 4)]
        //public AudioEncoderConfiguration AudioEncoderConfiguration { get; set; }

        //[XmlElement("VideoAnalyticsConfiguration", Order = 5)]
        //public VideoAnalyticsConfiguration VideoAnalyticsConfiguration { get; set; }

        //[XmlElement("PTZConfiguration", Order = 6)]
        //public PTZConfiguration PTZConfiguration { get; set; }

        //[XmlElement("MetadataConfiguration", Order = 7)]
        //public MetadataConfiguration MetadataConfiguration { get; set; }

        //[XmlElement("Extension", Order = 8)]
        //public ProfileExtension Extension { get; set; }

        [XmlAttribute]
        public string token { get; set; }

        [XmlAttribute]
        public bool @fixed { get; set; }
    }
}
