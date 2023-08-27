
using System;
using Project.Scripts.Connectivity.Models.KRLValues;

namespace Project.Scripts.Connectivity.Models.Wrappers
{
    public class KRLIntWrapper : IKRLWrapper<KRLValue>
    {
        public event EventHandler<KRLInt> ValueUpdated;
        
        private KRLInt krlInt;

        public KRLIntWrapper()
        {
            krlInt = new KRLInt();
        }

        public void UpdateValue(KRLValue update)
        {
            krlInt = (KRLInt)update;
            OnValueUpdated(krlInt);
        }

        protected virtual void OnValueUpdated(KRLInt e)
        {
            ValueUpdated?.Invoke(this, e);
        }
    }
}
