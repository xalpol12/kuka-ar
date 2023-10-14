using Newtonsoft.Json;
using UnityEngine;

namespace Project.Scripts.Connectivity.Models.KRLValues
{
    public struct KRLFrame : IKRLValue
    {
        private Vector3 position;
    
        [JsonProperty("position")]
        public Vector3 Position
        {
            get => position;
            set
            {
                var kukaToUnityVector3 = new Vector3(-value.x, value.z, -value.y) / 1000f; // KUKA -> UNITY
                position = kukaToUnityVector3;
            } 
        }

        [JsonProperty("rotation")]
        public Vector3 Rotation { get; set; }
    }
}
