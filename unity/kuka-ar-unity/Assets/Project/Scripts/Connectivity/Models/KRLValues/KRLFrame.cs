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
                var kukaToUnityVector3 = new Vector3(
                    value.y / 1000,
                    value.x / 1000, 
                    value.z / 1000);
                position = kukaToUnityVector3;
            } 
        }

        private Vector3 rotation;
        
        [JsonProperty("rotation")]
        public Vector3 Rotation
        {
            get => rotation;
            
            set => rotation = Quaternion.Euler(new Vector3(0, 0, 90)) * value;
        }
    }
}
