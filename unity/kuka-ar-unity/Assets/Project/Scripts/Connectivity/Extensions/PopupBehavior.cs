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
            } else if (controller.GrabState == 0)
            {
                StartCoroutine(AutoPull());
                controller.GrabState = 2;
            } 
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

        internal IEnumerator SlideIn(GameObject notification, PopupContent content)
        {
            var stop = notification.transform.GetSiblingIndex() > 1
                ? Screen.height + (TravelDistance * (notification.transform.GetSiblingIndex() - 2))
                : Screen.height - TravelDistance * notification.transform.GetSiblingIndex() - 1;
            
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
            var pivot = new Vector2(Input.mousePosition.x / (Screen.width - 80), 0.5f);
            controller.Notifications[0].transform.GetComponent<RectTransform>().pivot = pivot;
            
            while (controller.InternalGrabState == 1)
            {
                controller.Notifications[0].transform.position = new Vector3(Input.mousePosition.x,
                                controller.Notifications[0].transform.position.y);
                yield return null;
            }

            controller.GrabState = 0;
            controller.InternalGrabState = 2;
        }

        private IEnumerator AutoPull()
        {
            while (controller.InternalGrabState == 2)
            {
                Vector3 pullInto;
                if (controller.Notifications[0].transform.position.x < Screen.width * 0.25)
                {
                    pullInto = Vector3.left;
                } else if (controller.Notifications[0].transform.position.x > Screen.width * 0.75)
                {
                    pullInto = Vector3.right;
                }
                else
                {
                    yield return null;
                    continue;
                }
                
                controller.Notifications[0].transform.Translate(Time.deltaTime * transformFactor * pullInto);
                yield return null;
            }

            controller.InternalGrabState = 0;
        }
    }
}
