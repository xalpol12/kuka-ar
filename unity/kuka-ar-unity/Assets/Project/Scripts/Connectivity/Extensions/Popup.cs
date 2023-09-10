using System;
using System.Collections;
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
        /// </summary>
        public void Try(Action action,bool withPopup = false)
        {
            try
            {
                action();
                if (withPopup)
                {
                    
                    StartCoroutine(ShowNotification());
                }
                return;
            }
            catch (Exception e)
            {
                DefaultContent("Error", e.Message, watcher.Wifi);
                
                if (e is WebException or HttpRequestException or SocketException or AggregateException)
                {
                    content.Message = e.InnerException?.InnerException?.Message.Split("(")[1];
                    content.Icon = watcher.NoWifi;
                    if (HasDuplicates()) return;
                }
            }

            StartCoroutine(ShowNotification());
        }

        private IEnumerator ShowNotification()
        {
            var newPopup = Instantiate(notification, notification.transform.parent, true);
            Notifications.Add(newPopup);
            
            NotificationsContent.Add(content);
            StartCoroutine(popupBehavior.SlideIn(newPopup, content));
            yield return null;
        }

        private void DefaultContent(string header,string message, Sprite icon)
        {
            content = new PopupContent
            {
                Header = header.RemoveDiacritics(),
                Message = message.RemoveDiacritics(),
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

        private bool HasDuplicates()
        {
            for (var i = 0; i < NotificationsContent.Count - 1; i++)
            {
                if (NotificationsContent[^1].Message == content.Message)
                {
                    return true;
                }
            }

            return false;
        }

        private void DetectOperationResult()
        {
            
        }
    }
}
