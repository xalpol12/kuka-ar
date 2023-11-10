using System;
using Project.Scripts.Connectivity.Enums;
using Project.Scripts.EventSystem.Enums;
using UnityEngine;

namespace Project.Scripts.EventSystem.Services.Menu
{
    public class SelectableLogicService : MonoBehaviour
    {
        public static SelectableLogicService Instance;
        
        public ConnectionStatus robotConnectionStatus { get; set; } = ConnectionStatus.Disconnected;

        [NonSerialized] public LogicStates SliderState;
        [NonSerialized] public string SelectedIpAddress;
        [NonSerialized] public string SelectedName;
        [NonSerialized]public bool IsAfterItemSelect;

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
