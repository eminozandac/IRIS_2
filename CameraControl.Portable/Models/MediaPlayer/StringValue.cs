using System.Xml.Serialization;

namespace CameraControl.Portable.Models.MediaPlayer
{
    public class StringValue
    {
        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
