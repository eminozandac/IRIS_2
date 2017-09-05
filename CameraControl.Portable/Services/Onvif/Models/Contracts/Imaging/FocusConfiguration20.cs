using System.Xml.Serialization;
using CameraControl.Portable.Services.Onvif.Constants;

namespace CameraControl.Portable.Services.Onvif.Models.Contracts.Imaging
{
    [XmlType("FocusConfiguration20", Namespace = Namespaces.OnvifSchema)]
    public class FocusConfiguration20
    {
        [XmlElement(Order = 0)]
        public AutoFocusMode AutoFocusMode { get; set; }
        
        [XmlElement(Order = 1)]
        public float? DefaultSpeed { get; set; }
        
        [XmlElement(Order = 2)]
        public float? NearLimit { get; set; }
        
        [XmlElement(Order = 3)]
        public float? FarLimit { get; set; }
        
        //[XmlElement(Order = 4)]
        //public FocusConfiguration20Extension Extension { get; set; }
    }
}
