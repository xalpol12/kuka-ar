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
        
        [NonSerialized] public LogicStates State;
        [NonSerialized] public bool IsSliderHold;
        private void Start()
        {
            CoordSelectMenu = GetComponent<RectTransform>()
                .Find("Selectable").GetComponent<RectTransform>().gameObject;
            ConstantTopPanel = GetComponent<RectTransform>()
                .Find("ConstantInfoPanel").GetComponent<RectTransform>().gameObject;

            State = LogicStates.Waiting;
            IsSliderHold = false;
            
            TopMenuEvents.Events.OnBeanClick += OnToolOrBaseClick;
            TopMenuEvents.Events.OnDragTopMenuSlider += OnMenuDrag;
            TopMenuEvents.Events.OnDropTopMenuSlider += OnMenuDrop;
        }

        private void OnToolOrBaseClick(int uid)
        {
            if (id == uid)
            {
                State = LogicStates.Running;
            }
        }

        private void OnMenuDrag(int uid)
        {
            Debug.Log("Drag init");
            if (id == uid)
            {
                Debug.Log("Drag state");
                IsSliderHold = true;
            }
        }

        private void OnMenuDrop(int uid)
        {
            Debug.Log("Drop state");
            if (id == uid)
            {
                Debug.Log("Drop state");
                IsSliderHold = false;
            }
        }
    }
}
