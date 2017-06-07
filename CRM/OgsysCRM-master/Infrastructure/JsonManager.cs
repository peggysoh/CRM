using Newtonsoft.Json;

namespace OgsysCRM.Infrastructure
{
    public class JsonManager
    {
        public static T DeSerialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}