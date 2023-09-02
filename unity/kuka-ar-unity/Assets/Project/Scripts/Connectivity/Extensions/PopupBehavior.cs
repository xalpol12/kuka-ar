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
        
        private const int TravelDistance = 250;

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
                ? Screen.height + (220 * (notification.transform.GetSiblingIndex() - 2))
                : Screen.height - 220 * notification.transform.GetSiblingIndex() - 1;
            
            while (notification.transform.position.y > stop)
            {
                AssignContent(notification, content);
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
    }
}
