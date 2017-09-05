using System.Xml.Serialization;

namespace CameraControl.Portable.Models.MediaPlayer
{
    public class DecimalValue
    {
        [XmlAttribute("value")]
        public decimal Value { get; set; }
    }
}
