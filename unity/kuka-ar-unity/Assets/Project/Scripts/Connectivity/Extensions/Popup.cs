using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using Newtonsoft.Json;
using Project.Scripts.Connectivity.Enums;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.Connectivity.Models.SimpleValues.Pairs;
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
        public void Try(Action action,Robot result = default, RequestType type = RequestType.PATCH)
        {
            try
            {
                action();
                if (!Equals(result, default(Robot)))
                {
                    DetectOperationType(result, type);
                    StartCoroutine(ShowNotification());
                }
                return;
            }
            catch (Exception e)
            {
                DefaultContent("Error", e.Message, watcher.Wifi);

                switch (e)
                {
                    case WebException or SocketException or AggregateException:
                    {
                        content.Message = e.InnerException?.InnerException?.Message.Split("(")[1];
                        content.Icon = watcher.NoWifi;
                        if (HasDuplicates()) return;
                        break;
                    }
                    case HttpRequestException:
                        try
                        {
                            var error = JsonConvert.DeserializeObject<ExceptionMessagePair>(e.Message);
                            content = new PopupContent
                            {
                                Header = $"{error.ExceptionName} {error.ExceptionCode}",
                                Message = error.ExceptionMessage,
                                Icon = type is RequestType.POST or RequestType.PUT 
                                    ? watcher.AddedFailed : watcher.NoWifi,
                            };
                        }
                        catch (JsonReaderException jsonReaderException)
                        {
                            DefaultContent("Http request error", jsonReaderException.Message, watcher.AddedFailed);
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine($"Error occured. {exception.Message}");
                        }

                        break;
                }
            }
            
            SetTimestamp();
            StartCoroutine(ShowNotification());
        }
        
        /// <summary>
        /// Allows to destroy popup.
        /// @param @optional index - element index in popup collection
        /// </summary>
        public void Discard(int index = -1)
        {
            var itemIndex = index == -1 ? NotificationsContent.Count - 1 : index;
            PopupBehavior.DeleteItem(Notifications[itemIndex]);
            Notifications.RemoveAt(itemIndex);
            NotificationsContent.RemoveAt(itemIndex);
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
                Icon = icon,
            };
            
            SetTimestamp();
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

        private void DetectOperationType(Robot response, RequestType type)
        {
            content = type switch
            {
                RequestType.POST => new PopupContent
                {
                    Header = "Robot added",
                    Message = $"Machine with name {response.Name} has been added",
                    Icon = watcher.AddedSuccess
                },
                RequestType.PUT => new PopupContent
                {
                    Header = "Robots data updated",
                    Message = $"Successfully updated robot with ip address {response.IpAddress}",
                    Icon = watcher.EditSuccess
                },
                _ => content
            };

            SetTimestamp();
        }

        private void SetTimestamp()
        {
            content.Timestamp = DateTime.Now.ToString("HH:mm");
            content.DateTimeMark = DateTime.Now;
        }
    }
}
