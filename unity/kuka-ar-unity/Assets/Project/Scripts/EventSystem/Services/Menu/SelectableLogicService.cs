using Project.Scripts.EventSystem.Enums;
using UnityEngine;

namespace Project.Scripts.EventSystem.Services.Menu
{
    public class SelectableLogicService : MonoBehaviour
    {
        public static SelectableLogicService Instance;
        
        internal LogicStates SliderState;
        internal string SelectedIpAddress;
        internal string SelectedName;
        internal bool IsAfterItemSelect;

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
