using Newtonsoft.Json;
using UnityEngine;

namespace Project.Scripts.Connectivity.Models.KRLValues
{
    public struct KRLFrame : IKRLValue
    {
        private Vector3 unityPosition;
    
        [JsonProperty("position")]
        public Vector3 Position
        {
            get => unityPosition; // UNITY -> KUKA
            set
            {
                var kukaToUnityVector3 = new Vector3(-value.x, value.z, -value.y) / 1000f; // KUKA -> UNITY
                unityPosition = kukaToUnityVector3;
            } 
        }

        [JsonIgnore]
        public Vector3 UnityPosition
        {
            get => unityPosition;
            set => unityPosition = value;
        }

        [JsonProperty("rotation")]
        public Vector3 Rotation { get; set; }

    }
}
