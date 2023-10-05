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
        private void Start()
        {
            CoordSelectMenu = GetComponent<RectTransform>()
                .Find("Selectable").GetComponent<RectTransform>().gameObject;
            ConstantTopPanel = GetComponent<RectTransform>()
                .Find("ConstantInfoPanel").GetComponent<RectTransform>().gameObject;

            State = LogicStates.Waiting;
            
            TopMenuEvents.Events.OnBeanClick += OnToolOrBaseClick;    
        }

        private void OnToolOrBaseClick(int uid)
        {
            if (id == uid)
            {
                State = LogicStates.Running;
            }
        }
    }
}
