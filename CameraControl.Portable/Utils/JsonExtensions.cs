using MvvmCross.Platform.Platform;

namespace CameraControl.Portable.Utils
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T @object, IMvxJsonConverter jsonConverter) where T : class
        {
            return jsonConverter.SerializeObject(@object);
        }

        public static T FromJson<T>(this string json, IMvxJsonConverter jsonConverter) where T : class
        {
            return jsonConverter.DeserializeObject<T>(json);
        }
    }
}
