using System.Collections.Generic;
using Newtonsoft.Json;
using Scenes.RotationAndScalingWithButtons.Scripts.Models.AggregationClasses;
using Scenes.RotationAndScalingWithButtons.Scripts.Models.SimpleValues.Pairs;

namespace Scenes.RotationAndScalingWithButtons.Scripts.Connectivity.Parsing.OutputJson
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
