using Newtonsoft.Json;

namespace Project.Scripts.Connectivity.Models.KRLValues
{
    public struct KRLJoints : IKRLValue
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

        public static KRLJoints operator -(KRLJoints left, KRLJoints right)
        {
            var result = new KRLJoints
            {
                J1 = left.J1 - right.J1,
                J2 = left.J2 - right.J2,
                J3 = left.J3 - right.J3,
                J4 = left.J4 - right.J4,
                J5 = left.J5 - right.J5,
                J6 = left.J6 - right.J6
            };
            return result;
        }
    }
}
