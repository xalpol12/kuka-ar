using UnityEngine;

namespace Project.Scripts.Connectivity.Models
{
    public struct RobotData {
        public string Name { get; set; }

        private Vector3 positionShift;

        public Vector3 PositionShift
        {
            get => positionShift;
            set
            {
                var kukaToUnityVector3 = new Vector3(-value.x, value.z, -value.y); // KUKA => UNITY
                positionShift = kukaToUnityVector3;
            }
        }

        public Vector3 RotationShift { get; set; }
    }
}
