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
        public event Action<int> OnDragTopMenuSlider;
        public event Action<int> OnDropTopMenuSlider; 

        public void OpenTopMenu(int id)
        {
            OnBeanClick?.Invoke(id);
        }

        public void DragTopMenu(int id)
        {
            OnDragTopMenuSlider?.Invoke(id);
        }

        public void DropTopMenu(int id)
        {
            OnDropTopMenuSlider?.Invoke(id);
        }
    }
}
