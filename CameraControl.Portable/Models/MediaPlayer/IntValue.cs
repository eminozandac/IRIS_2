using System.Xml.Serialization;

namespace CameraControl.Portable.Models.MediaPlayer
{
    public class IntValue
    {
        [XmlAttribute("value")]
        public int Value { get; set; }
    }
}
