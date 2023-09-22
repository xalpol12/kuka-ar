using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project.Scripts.Connectivity.Models.KRLValues;

namespace Project.Scripts.Connectivity.Parsing
{
    public class KrlValueJsonConverter : JsonConverter<IKrlValue>
    {
        public override IKrlValue ReadJson(JsonReader reader, Type objectType, IKrlValue existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var json = JObject.Load(reader);

            if (json.ContainsKey("valueInt"))
            {
                return json.ToObject<KrlInt>();
            }

            if (json.ContainsKey("position"))
            {
                return json.ToObject<KrlFrame>();
            }

            if (json.ContainsKey("j1"))
            {
                return json.ToObject<KrlJoints>();
            }

            throw new InvalidDataException();
        }

        public override void WriteJson(JsonWriter writer, IKrlValue value, JsonSerializer serializer)
        {
            var json = JObject.FromObject(value);
            json.WriteTo(writer);
        }
    }
}
