using System.Runtime.Serialization;

namespace Project.Scripts.Connectivity.Models.AggregationClasses
{
    public class AddRobotData
    {
        [DataMember(Name = "id")]
        private ulong Id { get; set; }
        [DataMember(Name = "ipAddress")]
        public string IpAddress { get; set; }
        [DataMember(Name = "category")]
        public string RobotCategory { get; set; }
        [DataMember(Name = "name")]
        public string RobotName { get; set; }
    }
}