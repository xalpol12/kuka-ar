using System;
using System.Collections;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Connectivity.Extensions
{
    public class PopupBehavior : MonoBehaviour
    {
        [SerializeField] [Range(1, 10000)] private int transformFactor = 100;
        
        private const int TravelDistance = 220;
        private Popup controller;

        private void Start()
        {
            controller = GetComponent<Popup>();
        }

        private void Update()
        {
            if (controller.GrabState == 1)
            {
                controller.InternalGrabState = 1;
                StartCoroutine(UserSlideHandler());
                controller.GrabState = 2;
            } else if (controller.GrabState == 0 && controller.PressedObject is not null)
            {
                StartCoroutine(AutoPull());
                controller.GrabState = 2;
            } 
        }

        public static void DeleteItem(GameObject deleteGameObject)
        {
            Destroy(deleteGameObject);
        }

        internal PopupContent ResetContent()
        {
            return new PopupContent
            {
                Header = "",
                Message = "",
                Timestamp = "now",
                Icon = null
            };
        }

        internal IEnumerator SlideIn(GameObject notification, PopupContent content, int? index = null)
        {
            int stop;
            if (index is null)
            {
                stop = notification.transform.GetSiblingIndex() > 1
                    ? Screen.height + (TravelDistance * (notification.transform.GetSiblingIndex() - 2))
                    : Screen.height - TravelDistance * notification.transform.GetSiblingIndex() - 1;
            }
            else
            {
                stop = Screen.height - TravelDistance * (controller.Notifications.Count - index.Value);
            }
            
            AssignContent(notification, content);
            
            while (notification.transform.position.y > stop)
            {
                notification.transform.Translate((Time.deltaTime * transformFactor) * Vector3.down);
                yield return null;
            }
            
            notification.transform.position = new Vector3(notification.transform.position.x, stop);
        }

        private void AssignContent(GameObject notification, PopupContent content)
        {
            notification.transform.Find("Header").GetComponent<TMP_Text>().text = content.Header;
            notification.transform.Find("Message").GetComponent<TMP_Text>().text = content.Message.RemoveDiacritics();
            notification.transform.Find("Timestamp").GetComponent<TMP_Text>().text = content.Timestamp;
            notification.transform.Find("Background").GetComponent<Image>().gameObject.transform
                .Find("Icon").GetComponent<Image>().sprite = content.Icon;
        }
        
        private IEnumerator UserSlideHandler()
        {
            if (controller.PressedObject is null)
            {
                yield break;
            }
            var frac = Input.mousePosition.x / (Screen.width - 80 - 
                                                (controller.HomePosition.x -
                                                 Math.Abs(controller.PressedObject.transform.position.x)));
            controller.PressedObject.transform.GetComponent<RectTransform>().pivot = new Vector2(frac ,0.5f);
            
            while (controller.InternalGrabState == 1)
            {
                controller.PressedObject.transform.position = new Vector3(Input.mousePosition.x,
                                controller.PressedObject.transform.position.y);
                yield return null;
            }

            controller.GrabState = 0;
            controller.InternalGrabState = 2;
            yield return null;
        }

        private IEnumerator AutoPull()
        {
            var save = controller.PressedObject.GetComponent<RectTransform>().pivot;
            controller.PressedObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            while (controller.InternalGrabState == 2)
            {
                Vector3 pullInto;
                if (controller.PressedObject.transform.position.x < Screen.width * 0.25)
                {
                    pullInto = Vector3.left;
                } else if (controller.PressedObject.transform.position.x > Screen.width * 0.75)
                {
                    pullInto = Vector3.right;
                }
                else
                {
                    controller.PressedObject.GetComponent<RectTransform>().pivot = save;
                    yield break;
                }
                
                controller.PressedObject.transform.Translate(Time.deltaTime * transformFactor * pullInto);
                yield return null;
            }

            var deleteObject = controller.PressedObject;
            var index = controller.Notifications.IndexOf(deleteObject) == -1 ?
                0 : controller.Notifications.IndexOf(deleteObject);

            controller.PressedObject = null;
            controller.InternalGrabState = 0;
            controller.Notifications.Remove(deleteObject);
            controller.NotificationsContent.RemoveAt(index);
            
            Destroy(deleteObject);
            StartCoroutine(SlideDownAfterDelete(index));
            yield return null;
        }

        private IEnumerator SlideDownAfterDelete(int index)
        {
            for (var i = 0; i < controller.Notifications.Count; i++)
            {
                StartCoroutine(SlideIn(controller.Notifications[i], controller.NotificationsContent[i], i));
                yield return null;
            }
        }
    }
}
