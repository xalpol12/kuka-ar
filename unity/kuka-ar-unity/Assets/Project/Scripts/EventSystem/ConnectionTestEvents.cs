using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Project.Scripts.EventSystem
{
    public class ConnectionTestEvents : MonoBehaviour
    {
        public static ConnectionTestEvents Current;

        private void Awake()
        {
            Current = this;
        }

        public event Action OnPressButtonConnectToFirstRobot;
        public event Action OnPressButtonConnectToSecondRobot;
        public event Action OnPressButtonConnectToThirdRobot;

        public void StartConnectionFirstRobot()
        {
            OnPressButtonConnectToFirstRobot?.Invoke();
        }
        
        public void StartConnectionSecondRobot()
        {
            OnPressButtonConnectToSecondRobot?.Invoke();
        }
        
        public void StartConnectionThirdRobot()
        {
            OnPressButtonConnectToThirdRobot?.Invoke();
        }
    }
}
