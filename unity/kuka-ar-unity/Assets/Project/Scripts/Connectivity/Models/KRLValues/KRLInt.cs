using Newtonsoft.Json;

namespace Connectivity.Models.KRLValues
{
    public class KRLInt : KRLValue
    {
        [JsonProperty("valueInt")]
        public int Value { get; set; }
    }
}
