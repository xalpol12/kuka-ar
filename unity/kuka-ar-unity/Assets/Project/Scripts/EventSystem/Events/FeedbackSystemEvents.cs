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
    }
}
