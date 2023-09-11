using System;
using UnityEngine;

namespace Project.Scripts.EventSystem.DebugEventSystem
{
    public class JogsTestEvents : MonoBehaviour
    {
        public static JogsTestEvents Current;

        private void Awake()
        {
            Current = this;
        }

        public event Action OnPressButtonConnectToServer;
        public event Action OnPressButtonConnectToTwoRobots;
        public event Action OnPressButtonSwitchIP;

        public void StartConnectionToServer()
        {
            OnPressButtonConnectToServer?.Invoke();
        }

        public void ConnectToTwoRobots()
        {
            OnPressButtonConnectToTwoRobots?.Invoke();
        }

        public void SwitchIP()
        {
            OnPressButtonSwitchIP?.Invoke();
        }
    }
}
