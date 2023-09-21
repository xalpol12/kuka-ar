using System;
using Project.Scripts.Connectivity.Models.KRLValues;

namespace Project.Scripts.Connectivity.Models.Wrappers
{
    public class KrlJointsWrapper : IKrlWrapper
    {
        public event EventHandler<KrlJoints> ValueUpdated;
        
        private KrlJoints krlLocalJoints { get; set; }

        public KrlJoints KrlJoints => krlLocalJoints;

        private readonly float rotationThreshold;

        public KrlJointsWrapper(float rotationThreshold)
        {
            this.rotationThreshold = rotationThreshold;
            krlLocalJoints = new KrlJoints();
        }

        public void UpdateValue(IKrlValue update)
        {
            var newValue = (KrlJoints)update;
            if (!IsNewValueGreaterThanRotationThreshold(newValue)) return;
            krlLocalJoints = newValue;
            OnValueUpdated(krlLocalJoints);
        }

        private bool IsNewValueGreaterThanRotationThreshold(KrlJoints newValue)
        {
            var difference = newValue - krlLocalJoints;
            return Math.Abs(difference.J1) > rotationThreshold ||
                   Math.Abs(difference.J2) > rotationThreshold ||
                   Math.Abs(difference.J3) > rotationThreshold ||
                   Math.Abs(difference.J4) > rotationThreshold ||
                   Math.Abs(difference.J5) > rotationThreshold ||
                   Math.Abs(difference.J6) > rotationThreshold;
        }

        private void OnValueUpdated(KrlJoints e)
        {
            ValueUpdated?.Invoke(this, e);
        }
    }
}
