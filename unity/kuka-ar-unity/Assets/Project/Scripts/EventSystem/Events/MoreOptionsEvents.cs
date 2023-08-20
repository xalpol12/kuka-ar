using System;
using UnityEngine;

namespace Project.Scripts.EventSystem.Events
{
    public class MoreOptionsEvents : MonoBehaviour
    {
        public static MoreOptionsEvents Events;

        private void Awake()
        {
            Events = this;
        }

        public event Action<int> OnClickBack;
        public event Action<int> OnClickDisplayServer;
        public event Action<int> OnClickDisplayBrowser;

        public void BackToMenu(int id)
        {
            OnClickBack?.Invoke(id);
        }

        public void DisplayServerConfigurationWindow(int id)
        {
            OnClickDisplayServer?.Invoke(id);
        }

        public void DisplayIssueWebPage(int id)
        {
            OnClickDisplayBrowser?.Invoke(id);
        }
    }
}
