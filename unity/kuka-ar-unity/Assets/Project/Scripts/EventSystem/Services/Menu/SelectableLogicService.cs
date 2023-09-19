using Project.Scripts.EventSystem.Enums;
using UnityEngine;

namespace Project.Scripts.EventSystem.Services.Menu
{
    public class SelectableLogicService : MonoBehaviour
    {
        public static SelectableLogicService Instance;
        
        public ConnectionStatus RobotConnectionStatus { get; set; } = ConnectionStatus.Disconnected;

        public LogicStates SliderState;
        public string SelectedIpAddress;
        public string SelectedName;
        public bool IsAfterItemSelect;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            IsAfterItemSelect = false;
            SliderState = LogicStates.Waiting;
        }
    }
}
