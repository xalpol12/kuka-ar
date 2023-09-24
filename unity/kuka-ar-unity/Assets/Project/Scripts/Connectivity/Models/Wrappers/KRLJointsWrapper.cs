using System;
using Project.Scripts.Connectivity.Models.KRLValues;

namespace Project.Scripts.Connectivity.Models.Wrappers
{
    public class KrlJointsWrapper : IKrlWrapper
    {
        public event EventHandler<KRLJoints> ValueUpdated;
        
        private KRLJoints krlLocalJoints { get; set; }

        public KRLJoints KrlJoints => krlLocalJoints;

        private readonly float rotationThreshold;

        public KrlJointsWrapper(float rotationThreshold)
        {
            this.rotationThreshold = rotationThreshold;
            krlLocalJoints = new KRLJoints();
        }

        public void UpdateValue(IKRLValue update)
        {
            var newValue = (KRLJoints)update;
            if (!IsNewValueGreaterThanRotationThreshold(newValue)) return;
            krlLocalJoints = newValue;
            OnValueUpdated(krlLocalJoints);
        }

        private bool IsNewValueGreaterThanRotationThreshold(KRLJoints newValue)
        {
            var difference = newValue - krlLocalJoints;
            return Math.Abs(difference.J1) > rotationThreshold ||
                   Math.Abs(difference.J2) > rotationThreshold ||
                   Math.Abs(difference.J3) > rotationThreshold ||
                   Math.Abs(difference.J4) > rotationThreshold ||
                   Math.Abs(difference.J5) > rotationThreshold ||
                   Math.Abs(difference.J6) > rotationThreshold;
        }

        private void OnValueUpdated(KRLJoints e)
        {
            ValueUpdated?.Invoke(this, e);
        }
    }
}
