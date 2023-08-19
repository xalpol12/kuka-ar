using Unity.Plastic.Newtonsoft.Json;

namespace Project.Scripts.Connectivity.Models.KRLValues
{
    public class KRLJoints : KRLValue
    {
        [JsonProperty("j1")]
        public double J1 { get; set; }
        [JsonProperty("j2")]
        public double J2 { get; set; }
        [JsonProperty("j3")]
        public double J3 { get; set; }
        [JsonProperty("j4")]
        public double J4 { get; set; }
        [JsonProperty("j5")]
        public double J5 { get; set; }
        [JsonProperty("j6")]
        public double J6 { get; set; }
    }
}
