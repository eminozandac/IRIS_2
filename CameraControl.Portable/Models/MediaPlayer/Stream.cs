using System.Xml.Serialization;

namespace CameraControl.Portable.Models.MediaPlayer
{
    [XmlType("Stream")]
    public class Stream
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlArray("VideoStreams")]
        public VideoStream[] VideoStreams { get; set; }
    }
}
