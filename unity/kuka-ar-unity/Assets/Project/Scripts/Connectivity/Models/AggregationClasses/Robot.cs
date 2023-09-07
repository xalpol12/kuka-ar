using Newtonsoft.Json;

namespace Project.Scripts.Connectivity.Models.AggregationClasses
{
    public struct Robot
    {
        [JsonProperty("id")]
        private ulong Id { get; set; }
    
        [JsonProperty("name")]
        public string Name { get; set; }
    
        [JsonProperty("category")]
        public string Category { get; set; }
    
        [JsonProperty("ipAddress")]
        public string IpAddress { get; set; }
    }
}
