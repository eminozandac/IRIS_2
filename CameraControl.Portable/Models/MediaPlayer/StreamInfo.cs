using System.Xml.Serialization;

namespace CameraControl.Portable.Models.MediaPlayer
{
    [XmlRoot("StreamInfo")]
    public class StreamInfo
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        public Stream Stream { get; set; }
    }
}
