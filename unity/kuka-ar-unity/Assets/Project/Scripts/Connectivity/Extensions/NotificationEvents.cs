using System;
using UnityEngine;

namespace Project.Scripts.Connectivity.Extensions
{
    public class NotificationEvents : MonoBehaviour
    {
        public static NotificationEvents Events;

        private void Awake()
        {
            Events = this;
        }

        public event Action<GameObject> DragNotification;
        public event Action<int> DropNotification;

        public void OnDragNotification(GameObject id)
        {
            DragNotification?.Invoke(id);
        }

        public void OnDropNotification(int id)
        {
            DropNotification?.Invoke(id);
        }
    }
}
