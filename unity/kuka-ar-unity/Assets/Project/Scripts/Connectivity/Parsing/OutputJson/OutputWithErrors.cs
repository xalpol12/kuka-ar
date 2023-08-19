using System.Collections.Generic;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.Connectivity.Models.SimpleValues.Pairs;
using Unity.Plastic.Newtonsoft.Json;

namespace Project.Scripts.Connectivity.Parsing.OutputJson
{
    public struct OutputWithErrors {
        
        [JsonProperty("values")]
        public Dictionary<string, Dictionary<string, ValueWithError>> Values { get; set; }
        
        [JsonProperty("exception")]
        public ExceptionMessagePair? Exception { get; set; }

        public override string ToString()
        {
            return $"{nameof(Values)}: {Values}, {nameof(Exception)}: {Exception}";
        }
    }
}
