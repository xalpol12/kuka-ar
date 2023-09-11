using System;
using Project.Scripts.Connectivity.Models.KRLValues;

namespace Project.Scripts.Connectivity.Models.Wrappers
{
    public interface IKRLWrapper
    {
        public void UpdateValue(KRLValue update);
    }
}
