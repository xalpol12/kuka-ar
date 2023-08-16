using System.Collections.Generic;
using Connectivity.Models.AggregationClasses;
using Connectivity.Models.SimpleValues.Pairs;
using Newtonsoft.Json;

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
