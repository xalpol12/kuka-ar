using Newtonsoft.Json;

namespace Project.Scripts.Connectivity.Models.KRLValues
{
    public class KRLInt : KRLValue
    {
        [JsonProperty("valueInt")]
        public int Value { get; set; }
    }
}
