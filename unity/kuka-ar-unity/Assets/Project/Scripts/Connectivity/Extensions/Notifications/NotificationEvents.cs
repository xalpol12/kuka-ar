using System;
using UnityEngine;

namespace Project.Scripts.Connectivity.Extensions.Notifications
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

        public void OnDragNotification(GameObject obj)
        {
            DragNotification?.Invoke(obj);
        }

        public void OnDropNotification(int id)
        {
            DropNotification?.Invoke(id);
        }
    }
}
