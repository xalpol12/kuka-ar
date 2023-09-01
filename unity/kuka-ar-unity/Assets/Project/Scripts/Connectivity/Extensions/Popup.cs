using System;
using System.Collections;
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
        private GameObject popupWindow;
        
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
            dialogWindow = popupWindow.transform.GetComponent<RectTransform>();
            scaleFactor = 0.01f;
            error = Resources.Load<Sprite>("Icons/cloudFailedIcon");
            popupWindow.SetActive(false);
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
                    StartCoroutine(ScaleUp());
                }
            }
        }

        private IEnumerator ScaleUp()
        {
            popupWindow.SetActive(true);
            while (dialogWindow.transform.localScale.y < 1)
            {
                dialogWindow.transform.localScale += new Vector3(scaleFactor, scaleFactor);
                yield return null;
            }
        }
        
        public IEnumerator ScaleDown()
        {
            while (popupWindow.transform.localScale.y > 0.01)
            {
                popupWindow.transform.localScale -= new Vector3(scaleFactor, scaleFactor);
                yield return null;
            }
            popupWindow.SetActive(false);
        }
    }
}
