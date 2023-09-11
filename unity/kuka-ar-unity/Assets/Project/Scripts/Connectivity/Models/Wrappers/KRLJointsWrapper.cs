
using System;
using Project.Scripts.Connectivity.Models.KRLValues;

namespace Project.Scripts.Connectivity.Models.Wrappers
{
    public class KRLJointsWrapper : IKRLWrapper
    {
        public event EventHandler<KRLJoints> ValueUpdated;
        
        private KRLJoints krlJoints { get; set; }

        public KRLJoints KrlJoints => krlJoints;

        private readonly float rotationThreshold;

        public KRLJointsWrapper(float rotationThreshold)
        {
            this.rotationThreshold = rotationThreshold;
            krlJoints = new KRLJoints();
        }

        public void UpdateValue(KRLValue update)
        {
            KRLJoints newValue = (KRLJoints)update;
            if (IsNewValueGreaterThanRotationThreshold(newValue)) 
            {
                krlJoints = newValue;
                OnValueUpdated(krlJoints);
            }
        }

        private bool IsNewValueGreaterThanRotationThreshold(KRLJoints newValue)
        {
            var difference = newValue - krlJoints;
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
