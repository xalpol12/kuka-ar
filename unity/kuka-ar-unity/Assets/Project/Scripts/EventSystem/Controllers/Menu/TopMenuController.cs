using System;
using Project.Scripts.EventSystem.Enums;
using Project.Scripts.EventSystem.Events;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class TopMenuController : MonoBehaviour
    {
        public int id;
        public int transformFactor;
        public float dropScreenHeight;
        public GameObject jogs;
        [NonSerialized] public GameObject CoordSelectMenu;
        [NonSerialized] public GameObject ConstantTopPanel;
        
        [NonSerialized] public LogicStates ConstantPanelState;
        [NonSerialized] public LogicStates CoordListState;
        [NonSerialized] public bool IsSliderHold;
        private void Start()
        {
            CoordSelectMenu = GetComponent<RectTransform>()
                .Find("Selectable").GetComponent<RectTransform>().gameObject;
            ConstantTopPanel = GetComponent<RectTransform>()
                .Find("ConstantInfoPanel").GetComponent<RectTransform>().gameObject;

            ConstantPanelState = LogicStates.Waiting;
            CoordListState = LogicStates.Waiting;
            IsSliderHold = false;
            
            TopMenuEvents.Events.OnBeanClick += OnToolOrBaseClick;
            TopMenuEvents.Events.OnDragTopMenuSlider += OnMenuDrag;
            TopMenuEvents.Events.OnDropTopMenuSlider += OnMenuDrop;
        }

        private void OnToolOrBaseClick(int uid)
        {
            if (id == uid)
            {
                ConstantPanelState = LogicStates.Running;
            }
        }

        private void OnMenuDrag(int uid)
        {
            if (id == uid)
            {
                CoordListState = LogicStates.Running;
            }
        }

        private void OnMenuDrop(int uid)
        {
            if (id == uid)
            {
                CoordListState = LogicStates.AutoPulling;
            }
        }
    }
}
