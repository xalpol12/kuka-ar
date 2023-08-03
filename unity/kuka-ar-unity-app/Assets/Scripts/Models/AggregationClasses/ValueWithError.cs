using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Scenes.RotationAndScalingWithButtons.Scripts.Models.KRLValues;
using Scenes.RotationAndScalingWithButtons.Scripts.Models.SimpleValues.Pairs;

namespace Scenes.RotationAndScalingWithButtons.Scripts.Models.AggregationClasses
{
    public struct ValueWithError
    {
        [JsonProperty("value")]
        public KRLValue Value { get; set; }
        
        [JsonProperty("readExceptions")]
        public HashSet<ExceptionMessagePair> FoundExceptions { get; set; }
    }
}
