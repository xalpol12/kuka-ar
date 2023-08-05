using Connectivity.Models.KRLValues;

namespace Connectivity.Models
{
    public struct RobotData {
        public KRLInt CurrentInt { get; set; }
        public KRLFrame CurrentFrame { get; set; }
        public KRLJoints CurrentJoints { get; set; }
    }
}
