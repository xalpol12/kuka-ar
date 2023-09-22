
using System;
using Project.Scripts.Connectivity.Models.KRLValues;

namespace Project.Scripts.Connectivity.Models.Wrappers
{
    public class KrlIntWrapper : IKrlWrapper
    {
        public event EventHandler<KrlInt> ValueUpdated;
        
        private KrlInt krlInt;

        public void UpdateValue(IKrlValue update)
        {
            if (((KrlInt)update).Value == krlInt.Value) return;
            krlInt = (KrlInt)update;
            OnValueUpdated(krlInt);
        }

        private void OnValueUpdated(KrlInt e)
        {
            ValueUpdated?.Invoke(this, e);
        }
    }
}
