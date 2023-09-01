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
        public static Popup Window;
        public PopupContent Content;
        [SerializeField] 
        private GameObject notification;
        
        [SerializeField]
        [Range(0.01f, 1)]
        private float scaleFactor;

        private Sprite error;
        
        private RectTransform dialogWindow;
        private void Awake()
        {
            Window = this;
        }

        private void Start()
        {
            dialogWindow = notification.transform.GetComponent<RectTransform>();
            scaleFactor = 0.01f;
            error = Resources.Load<Sprite>("Icons/cloudFailedIcon");
            notification.SetActive(false);
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
                if (e is WebException or HttpRequestException or SocketException or AggregateException)
                {
                    Content = new PopupContent
                    {
                        Title = "Error",
                        Header = "Data fetch error",
                        Message = e.Message,
                        Icon = error
                    };
                    //StartCoroutine(ScaleUp());
                }
            }
        }
    }
}
