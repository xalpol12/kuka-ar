using UnityEngine;

namespace Project.Scripts.EventSystem
{
    public class ConnectionTestController : MonoBehaviour
    {
        public bool FirstRobotConnected { get; private set; }
        public bool SecondRobotConnected { get; private set; }
        public bool ThirdRobotConnected { get; private set; }

        private void Start()
        {
            FirstRobotConnected = false;
            SecondRobotConnected = false;
            ThirdRobotConnected = false;

            ConnectionTestEvents.Current.OnPressButtonConnectToFirstRobot += InitializeConnectionFirstRobot;
            ConnectionTestEvents.Current.OnPressButtonConnectToSecondRobot += InitializeConnectionSecondRobot;
            ConnectionTestEvents.Current.OnPressButtonConnectToThirdRobot += InitializeConnectionThirdRobot;
        }

        private void InitializeConnectionFirstRobot()
        {
            if (FirstRobotConnected) return;
            FirstRobotConnected = true;
        }
    
        private void InitializeConnectionSecondRobot()
        {
            if (SecondRobotConnected) return;
            SecondRobotConnected = true;
        }
    
        private void InitializeConnectionThirdRobot()
        {
            if (ThirdRobotConnected) return;
            ThirdRobotConnected = true;
        }
    }
}
