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
                var kukaToUnityVector3 = new Vector3(-value.x, value.z, -value.y);
                //                          + new Vector3(0f, 0f, 0.4f); // KUKA => UNITY
                // // i want to subtract 0.4 y on start in Kuka coords, so i construct a Vector3(0f, 0f, 0.4f) in Unity coords
                positionShift = kukaToUnityVector3;
            }
        }

        public Vector3 RotationShift { get; set; }
    }
}
