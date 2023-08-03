using Newtonsoft.Json;

namespace Scenes.RotationAndScalingWithButtons.Scripts.Models.KRLValues
{
    public class KRLInt : KRLValue
    {
        [JsonProperty("valueInt")]
        public int Value { get; set; }
    }
}
