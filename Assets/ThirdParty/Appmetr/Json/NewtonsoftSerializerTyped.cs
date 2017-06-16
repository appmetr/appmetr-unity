#if UNITY_STANDALONE
using System;
using AppmetrCS.Serializations;
using Newtonsoft.Json;
using Object = System.Object;

namespace Appmetr.Unity.Json
{
    public class NewtonsoftSerializerTyped : IJsonSerializer
    {
        public static readonly NewtonsoftSerializerTyped Instance = new NewtonsoftSerializerTyped();
        
        private readonly JsonSerializerSettings _jsonSerializerSettings =
            new JsonSerializerSettings 
            {
                NullValueHandling = NullValueHandling.Ignore, 
                TypeNameHandling = TypeNameHandling.Auto
            };
        
        public String Serialize(Object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, _jsonSerializerSettings);
        }

        public T Deserialize<T>(String json)
        {
            return JsonConvert.DeserializeObject<T>(json, _jsonSerializerSettings);
        }
    }
}
#endif