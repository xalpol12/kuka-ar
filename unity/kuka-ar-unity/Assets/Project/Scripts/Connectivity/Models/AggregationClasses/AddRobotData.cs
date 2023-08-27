using Newtonsoft.Json;

namespace Project.Scripts.Connectivity.Models.AggregationClasses
{
    public class AddRobotData
    {
        [JsonProperty("id")]
        private ulong Id { get; set; }
        [JsonProperty("ipAddress")]
        public string IpAddress { get; set; }
        [JsonProperty("category")]
        public string RobotCategory { get; set; }
        [JsonProperty("name")]
        public string RobotName { get; set; }
    }
}