using Scenes.RotationAndScalingWithButtons.Scripts.Models.KRLValues;

namespace Scenes.RotationAndScalingWithButtons.Scripts.Models
{
    public struct RobotData {
        public KRLInt CurrentInt { get; set; }
        public KRLFrame CurrentFrame { get; set; }
        public KRLJoints CurrentJoints { get; set; }
    }
}
