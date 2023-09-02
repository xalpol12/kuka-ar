using System;
using System.Collections;
using System.Text;
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

        private void TimeStampHandler(PopupContent content)
        {
            content.Timestamp = content.DateTimeMark - DateTime.Now + "ago";
        }

        internal IEnumerator SlideIn(GameObject notification, PopupContent content)
        {
            while (notification.transform.position.y > Screen.height - TravelDistance)
            {
                TimeStampHandler(content);
                AssignContent(notification, content);
                notification.transform.Translate((Time.deltaTime * transformFactor) * Vector3.down);
                yield return null;
            }

            notification.transform.position = new Vector3(notification.transform.position.x,
                Screen.height - TravelDistance);
        }

        private void AssignContent(GameObject notification, PopupContent content)
        {
            var noAscii = Encoding.ASCII.GetString(
                Encoding.Convert(
                    Encoding.UTF8,
                    Encoding.GetEncoding(
                        Encoding.ASCII.EncodingName,
                        new EncoderReplacementFallback(string.Empty),
                        new DecoderExceptionFallback()
                    ),
                    Encoding.UTF8.GetBytes(content.Message)
                )
            );
            
            notification.transform.Find("Header").GetComponent<TMP_Text>().text = content.Header;
            notification.transform.Find("Message").GetComponent<TMP_Text>().text = content.Message.RemoveDiacritics();
            notification.transform.Find("Timestamp").GetComponent<TMP_Text>().text = content.Timestamp;
            notification.transform.Find("Background").GetComponent<Image>().gameObject.transform
                .Find("Icon").GetComponent<Image>().sprite = content.Icon;
        }
    }
}
