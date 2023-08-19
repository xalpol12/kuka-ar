using System;
using System.IO;
using Project.Scripts.Connectivity.Models.KRLValues;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;

namespace Project.Scripts.Connectivity.Parsing
{
    public class KRLValueJsonConverter : JsonConverter<KRLValue>
    {
        public override KRLValue ReadJson(JsonReader reader, Type objectType, KRLValue existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var json = JObject.Load(reader);

            if (json.ContainsKey("valueInt"))
            {
                return json.ToObject<KRLInt>();
            }

            if (json.ContainsKey("position"))
            {
                return json.ToObject<KRLFrame>();
            }

            if (json.ContainsKey("j1"))
            {
                return json.ToObject<KRLJoints>();
            }

            throw new InvalidDataException();
        }

        public override void WriteJson(JsonWriter writer, KRLValue value, JsonSerializer serializer)
        {
            var json = JObject.FromObject(value);
            json.WriteTo(writer);
        }
    }
}
