using System;
using UnityEngine;

namespace Project.Scripts.EventSystem.Events
{
    public class FeedbackSystemEvents : MonoBehaviour
    {
        public static FeedbackSystemEvents Events;

        private void Awake()
        {
            Events = this;
        }

        public event Action<int> OnClickHidePopup;

        public void HidePopup(int id)
        {
            Debug.Log("invoke");
            OnClickHidePopup?.Invoke(id);
        }
    }
}
