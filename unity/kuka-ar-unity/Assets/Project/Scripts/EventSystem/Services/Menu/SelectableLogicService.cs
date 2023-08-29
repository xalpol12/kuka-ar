using Project.Scripts.EventSystem.Enums;
using UnityEngine;

namespace Project.Scripts.EventSystem.Services.Menu
{
    public class SelectableLogicService : MonoBehaviour
    {
        public static SelectableLogicService Instance;
        
        internal LogicStates SliderState;
        internal string SelectedIpAddress;
        internal bool IsAfterItemSelect;
        internal bool IsAfterNewRobotSave;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            IsAfterItemSelect = false;
            IsAfterNewRobotSave = false;
            SliderState = LogicStates.Waiting;
        }
    }
}
