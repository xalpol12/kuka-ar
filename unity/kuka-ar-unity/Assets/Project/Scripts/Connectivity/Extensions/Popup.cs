using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using UnityEngine;

namespace Project.Scripts.Connectivity.Extensions
{
    /// <summary>
    /// Popup display utility class.
    /// </summary>
    public class Popup : MonoBehaviour
    {
        public int id;
        public static Popup Window;
        
        [SerializeField] 
        private GameObject notification;

        internal LogicStates GrabState;
        
        private PopupContent content;
        private NotificationAssetWatcher watcher;
        private PopupBehavior popupBehavior;

        private void Awake()
        {
            Window = this;
        }

        private void Start()
        {
            watcher = NotificationAssetWatcher.Watcher;
            popupBehavior = GetComponent<PopupBehavior>();

            content = popupBehavior.ResetContent();

            NotificationEvents.Events.DragNotification += DragPopup;
            NotificationEvents.Events.DropNotification += DropPopup;
        }

        /// <summary>
        /// Tries to execute the given action. If it fails, shows popup window with error message.
        /// </summary>
        public void Try(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                var newPopup = Instantiate(notification, notification.transform.parent, true);
                
                DefaultErrorContent(e.Message);
                
                if (e is WebException or HttpRequestException or SocketException or AggregateException)
                {
                    content.Message = e.InnerException?.InnerException?.Message.Split("(")[1];
                    content.Icon = watcher.NoWifi;
                }
                StartCoroutine(popupBehavior.SlideIn(newPopup, content));
            }
        }

        private void DefaultErrorContent(string message)
        {
            content = new PopupContent
            {
                Header = "Error",
                Message = message,
                Timestamp = DateTime.Now.ToString("HH:mm"),
                DateTimeMark = DateTime.Now,
                Icon = watcher.Wifi,
            };
        }

        private void DragPopup(int uid)
        {
            if (id == uid)
            {
                Debug.Log("drag");
            }
        }

        private void DropPopup(int uid)
        {
            if (id == uid)
            {
                Debug.Log("drop");
            }
        }
    }
}
