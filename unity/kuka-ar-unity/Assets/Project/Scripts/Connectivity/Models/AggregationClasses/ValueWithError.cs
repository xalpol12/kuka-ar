using System.Collections.Generic;
using Project.Scripts.Connectivity.Models.KRLValues;
using Project.Scripts.Connectivity.Models.SimpleValues.Pairs;
using Unity.Plastic.Newtonsoft.Json;

namespace Project.Scripts.Connectivity.Models.AggregationClasses
{
    public struct ValueWithError
    {
        [JsonProperty("value")]
        public KRLValue Value { get; set; }
        
        [JsonProperty("readExceptions")]
        public HashSet<ExceptionMessagePair> FoundExceptions { get; set; }
    }
}
