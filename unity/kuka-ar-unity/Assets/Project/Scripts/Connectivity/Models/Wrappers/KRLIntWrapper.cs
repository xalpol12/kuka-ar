
using System;
using Project.Scripts.Connectivity.Models.KRLValues;

namespace Project.Scripts.Connectivity.Models.Wrappers
{
    public class KRLIntWrapper : IKRLWrapper
    {
        public event EventHandler<KRLInt> ValueUpdated;
        
        private KRLInt krlInt;

        public KRLIntWrapper()
        {
            krlInt = new KRLInt();
        }

        public void UpdateValue(KRLValue update)
        {
            if (((KRLInt)update).Value != krlInt.Value)
            {
                krlInt = (KRLInt)update;
                OnValueUpdated(krlInt);
            }
        }

        private void OnValueUpdated(KRLInt e)
        {
            ValueUpdated?.Invoke(this, e);
        }
    }
}
