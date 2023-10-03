using System;
using System.Collections.Generic;
using Project.Scripts.Connectivity.Models.KRLValues;

namespace Project.Scripts.Connectivity.Models.Wrappers
{
    public class KrlFrameWrapper : IKrlWrapper
    {
        private KRLFrame krlFrame;

        private readonly Queue<KRLFrame> updates;

        private readonly float positionThreshold;
        private readonly float rotationThreshold;

        public KrlFrameWrapper(float positionThreshold, float rotationThreshold)
        {
            this.positionThreshold = positionThreshold;
            this.rotationThreshold = rotationThreshold;
            
            krlFrame = new KRLFrame();
            
            updates = new Queue<KRLFrame>();
        }

        public void UpdateValue(IKRLValue update)
        {
            var newValue = (KRLFrame)update;

            if (!IsNewValueGreaterThanThreshold(newValue)) return;
            updates.Enqueue(newValue);
            krlFrame = newValue;
        }

        public bool TryDequeue(out KRLFrame result)
        {
            return updates.TryDequeue(out result);
        }

        private bool IsNewValueGreaterThanThreshold(KRLFrame newValue)
        {
            return IsNewValueGreaterThanPositionThreshold(newValue) ||
                   IsNewValueGreaterThanRotationThreshold(newValue);
        }
        
        private bool IsNewValueGreaterThanPositionThreshold(KRLFrame newValue)
        {
            var difference = newValue.Position - krlFrame.Position;

            return Math.Abs(difference.x) > positionThreshold ||
                   Math.Abs(difference.y) > positionThreshold ||
                   Math.Abs(difference.z) > positionThreshold;
        }

        private bool IsNewValueGreaterThanRotationThreshold(KRLFrame newValue)
        {
            var difference = newValue.Rotation - krlFrame.Rotation;

            return Math.Abs(difference.x) > rotationThreshold ||
                   Math.Abs(difference.y) > rotationThreshold ||
                   Math.Abs(difference.z) > rotationThreshold;
        }
    }
}
