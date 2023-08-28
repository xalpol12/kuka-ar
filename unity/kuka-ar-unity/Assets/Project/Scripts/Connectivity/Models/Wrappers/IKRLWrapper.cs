using System;

namespace Project.Scripts.Connectivity.Models.Wrappers
{
    public interface IKRLWrapper<in T>
    {
        public void UpdateValue(T update);
    }
}
