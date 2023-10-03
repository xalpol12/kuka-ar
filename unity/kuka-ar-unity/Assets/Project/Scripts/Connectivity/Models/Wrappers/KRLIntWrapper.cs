
using System;
using Project.Scripts.Connectivity.Models.KRLValues;

namespace Project.Scripts.Connectivity.Models.Wrappers
{
    public class KrlIntWrapper : IKrlWrapper
    {
        public event EventHandler<KRLInt> ValueUpdated;
        
        private KRLInt krlInt;

        public void UpdateValue(IKRLValue update)
        {
            if (((KRLInt)update).Value == krlInt.Value) return;
            krlInt = (KRLInt)update;
            OnValueUpdated(krlInt);
        }

        private void OnValueUpdated(KRLInt e)
        {
            ValueUpdated?.Invoke(this, e);
        }
    }
}
