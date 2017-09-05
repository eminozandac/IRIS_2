using System.IO;
using System.Xml.Serialization;

namespace CameraControl.Portable.Utils
{
    public static class XmlExtensions
    {
        public static T FromXml<T>(this string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                return default(T);
            }

            using (var textWriter = new StringReader(xml))
            {
                var result = (T) new XmlSerializer(typeof(T)).Deserialize(textWriter);

                return result;
            }
        }
    }
}
