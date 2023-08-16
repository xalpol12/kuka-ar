using System.Collections.Generic;
using Connectivity.Models.SimpleValues.Pairs;
using Newtonsoft.Json;
using Project.Scripts.Connectivity.Models.KRLValues;

namespace Connectivity.Models.AggregationClasses
{
    public struct ValueWithError
    {
        [JsonProperty("value")]
        public KRLValue Value { get; set; }
        
        [JsonProperty("readExceptions")]
        public HashSet<ExceptionMessagePair> FoundExceptions { get; set; }
    }
}
