using System.Xml.Serialization;

namespace CameraControl.Portable.Models.MediaPlayer
{
    [XmlType("VideoStream")]
    public class VideoStream
    {
        [XmlElement("format")]
        public StringValue Format { get; set; }

        [XmlElement("width")]
        public IntValue Width { get; set; }

        [XmlElement("height")]
        public IntValue Height { get; set; }

        [XmlElement("fps")]
        public DecimalValue Fps { get; set; }
    }
}
