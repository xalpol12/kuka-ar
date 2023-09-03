using System;
using System.Collections.Generic;
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

        internal int GrabState;
        internal int InternalGrabState;
        internal GameObject PressedObject;
        internal Vector3 HomePosition;
        internal List<GameObject> Notifications;
        internal List<PopupContent> NotificationsContent;
        
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
            Notifications = new List<GameObject>();
            NotificationsContent = new List<PopupContent>();
            HomePosition = notification.transform.position;
            GrabState = 2;
            InternalGrabState = 2;
            PressedObject = null;
            
            content = popupBehavior.ResetContent();

            NotificationEvents.Events.DragNotification += DragPopup;
            NotificationEvents.Events.DropNotification += DropPopup;
        }


        /// <summary>
        /// Tries to execute the given action. If it fails, shows popup window with error message.
        /// @param action - task to execute
        /// @param @param customMessage - overrides system generated notification content message
        /// @param @param withSuccess - allows to display message on action success
        /// @param @optional firstIteration - param to avoid error where first item of notification was static
        /// </summary>
        public void Try(Action action, string customMessage = "", bool firstIteration = true)
        {
            var newPopup = Instantiate(notification, notification.transform.parent, true);
            Notifications.Add(newPopup);
            if (Notifications.Count == 1 && firstIteration)
            {
                Notifications = new List<GameObject>();
                Try(action, customMessage,  false);
            }

            try
            {
                action();
            }
            catch (Exception e)
            {
                DefaultContent("Error",e.Message, watcher.Wifi);
                
                if (e is WebException or HttpRequestException or SocketException or AggregateException)
                {
                    content.Message = e.InnerException?.InnerException?.Message.Split("(")[1];
                    content.Icon = watcher.NoWifi;
                }
            }
            
            NotificationsContent.Add(content);
            StartCoroutine(popupBehavior.SlideIn(newPopup, content));
        }

        public void TryWithSuccessExpected(Action action, string customMessage = "")
        {
            var newPopup = Instantiate(notification, notification.transform.parent, true);
            Notifications.Add(newPopup);
            DefaultContent("Success", "Robot added to database", watcher.AddedSuccess);
            
           try
            {
                action();
            }
            catch (Exception e)
            {
                DefaultContent("Error", e.Message, watcher.AddedFailed);
            }
            
            NotificationsContent.Add(content);
            StartCoroutine(popupBehavior.SlideIn(newPopup, content));
        }
        
        private void DefaultContent(string header,string message, Sprite icon)
        {
            content = new PopupContent
            {
                Header = header,
                Message = message,
                Timestamp = DateTime.Now.ToString("HH:mm"),
                DateTimeMark = DateTime.Now,
                Icon = icon,
            };
        }

        private void DragPopup(GameObject pressed)
        {
            GrabState = 1;
            PressedObject = pressed;
        }

        private void DropPopup(int uid)
        {
            if (id == uid)
            {
                GrabState = 0;
            }
        }
    }
}
