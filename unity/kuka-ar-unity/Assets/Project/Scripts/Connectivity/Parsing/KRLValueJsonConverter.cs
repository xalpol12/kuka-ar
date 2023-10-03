using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project.Scripts.Connectivity.Models.KRLValues;

namespace Project.Scripts.Connectivity.Parsing
{
    public class KrlValueJsonConverter : JsonConverter<IKRLValue>
    {
        public override IKRLValue ReadJson(JsonReader reader, Type objectType, IKRLValue existingValue, bool hasExistingValue,
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

        public override void WriteJson(JsonWriter writer, IKRLValue value, JsonSerializer serializer)
        {
            var json = JObject.FromObject(value);
            json.WriteTo(writer);
        }
    }
}
