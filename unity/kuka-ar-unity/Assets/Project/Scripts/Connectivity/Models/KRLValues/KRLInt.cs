using Newtonsoft.Json;

namespace Project.Scripts.Connectivity.Models.KRLValues
{
    public struct KRLInt : IKRLValue
    {
        [JsonProperty("valueInt")]
        public int Value { get; set; }
    }
}
