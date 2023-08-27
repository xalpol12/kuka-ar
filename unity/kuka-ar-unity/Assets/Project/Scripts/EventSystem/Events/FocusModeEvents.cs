using System;
using UnityEngine;

namespace Project.Scripts.EventSystem.Events
{
    public class FocusModeEvents : MonoBehaviour
    {
        public static FocusModeEvents Events;

        private void Awake()
        {
            Events = this;
        }

        public event Action<int> OnClickDisplayMoreOptions;

        public void DisplayMoreOptions(int id)
        {
            OnClickDisplayMoreOptions?.Invoke(id);
        }
    }
}
