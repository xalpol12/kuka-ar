using System;
using UnityEngine;

namespace Project.Scripts.EventSystem.Events
{
    public class TopMenuEvents : MonoBehaviour
    {
        public static TopMenuEvents Events;

        private void Awake()
        {
            Events = this;
        }

        public event Action<int> OnBeanClick;

        public void OpenTopMenu(int id)
        {
            OnBeanClick?.Invoke(id);
        }
    }
}
