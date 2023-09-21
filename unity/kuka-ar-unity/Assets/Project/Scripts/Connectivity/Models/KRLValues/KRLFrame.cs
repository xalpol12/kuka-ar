using Newtonsoft.Json;
using UnityEngine;

namespace Project.Scripts.Connectivity.Models.KRLValues
{
    public struct KrlFrame : IKrlValue
    {
        private Vector3 position;
    
        [JsonProperty("position")]
        public Vector3 Position
        {
            get => position;
            set
            {
                var kukaToUnityVector3 = new Vector3(
                    value.x / 1000,
                    value.y / 1000, 
                    value.z / 1000);
                position = kukaToUnityVector3;
            } 
        }
    
        [JsonProperty("rotation")]
        public Vector3 Rotation { get; set; }
    }
}
