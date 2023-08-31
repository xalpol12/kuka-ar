using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using UnityEngine;

namespace Project.Scripts.EventSystem.Extensions
{
    /// <summary>
    /// Popup display utility class.
    /// </summary>
    public class Popup : MonoBehaviour
    {
        public static Popup Window;

        private void Awake()
        {
            Window = this;
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
                    Debug.Log("FUTURE POPUP INFO DISPLAY");
                }
            }
        }
    }
}
