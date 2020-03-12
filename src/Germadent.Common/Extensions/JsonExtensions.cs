using Newtonsoft.Json;

namespace Germadent.Common.Extensions
{
    public static class JsonExtensions
    {
        public static string SerializeToJson(this object obj, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(obj, formatting);
        }

        public static T DeserializeFromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}