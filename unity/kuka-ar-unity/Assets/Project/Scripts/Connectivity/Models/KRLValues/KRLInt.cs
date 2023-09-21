using Newtonsoft.Json;

namespace Project.Scripts.Connectivity.Models.KRLValues
{
    public struct KrlInt : IKrlValue
    {
        [JsonProperty("valueInt")]
        public int Value { get; set; }
    }
}
