using UnityEngine;

namespace Project.Scripts.Connectivity.Models
{
    public struct RobotData {
        public string Name { get; set; }
        public Vector3 PositionShift { get; set; }
        public Vector3 RotationShift { get; set; }
    }
}
