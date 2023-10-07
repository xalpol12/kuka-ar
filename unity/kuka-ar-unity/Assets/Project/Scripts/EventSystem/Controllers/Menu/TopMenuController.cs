using System;
using Project.Scripts.Connectivity.Models.KRLValues;
using Project.Scripts.EventSystem.Enums;
using Project.Scripts.EventSystem.Events;
using Project.Scripts.TrackedRobots;
using TMPro;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class TopMenuController : MonoBehaviour
    {
        [Tooltip("Id of event controller")]
        public int id;
        
        [Tooltip("Animation speed")]
        public int transformFactor;
        
        [Tooltip("Menu drop height, counted from screen height [%]")]
        public float dropScreenHeight;
        
        [Tooltip("Jogs component reference object")]
        public GameObject jogs;
        
        [NonSerialized] public GameObject CoordSelectMenu;
        [NonSerialized] public GameObject ConstantTopPanel;
        
        [NonSerialized] public LogicStates ConstantPanelState;
        [NonSerialized] public LogicStates CoordListState;
        
        [SerializeField]
        [Tooltip("Tracked robots handler reference game object")]
        private GameObject robotModel;
        private TrackedRobotModel trackedRobot;
        
        private TMP_Text toolNo;
        private TMP_Text baseNo;

        private void Awake()
        {
            trackedRobot = robotModel.GetComponent<TrackedRobotsHandler>().CurrentlyTrackedRobot;
        }

        private void Start()
        {
            CoordSelectMenu = GetComponent<RectTransform>()
                .Find("Selectable").GetComponent<RectTransform>().gameObject;
            ConstantTopPanel = GetComponent<RectTransform>()
                .Find("ConstantInfoPanel").GetComponent<RectTransform>().gameObject;
            
            toolNo = ConstantTopPanel.transform.Find("ToolBean").GetComponent<RectTransform>().gameObject
                .transform.Find("Tool").GetComponent<TMP_Text>();
            baseNo = ConstantTopPanel.transform.Find("WorldBean").GetComponent<RectTransform>().gameObject
                .transform.Find("World").GetComponent<TMP_Text>();

            ConstantPanelState = LogicStates.Waiting;
            CoordListState = LogicStates.Waiting;
            
            TopMenuEvents.Events.OnBeanClick += OnToolOrBaseClick;
            TopMenuEvents.Events.OnDragTopMenuSlider += OnMenuDrag;
            TopMenuEvents.Events.OnDropTopMenuSlider += OnMenuDrop;
            
            if (trackedRobot is null) return; // TODO REMOVE THIS PART AFTER CONSULTATION WITH @xampol12
            trackedRobot.BaseValueUpdated += OnBaseValueChange;
            trackedRobot.ToolValueUpdated += OnToolValueChange;
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

        private void OnBaseValueChange(object _, KRLInt e)
        {
            baseNo.text = "Base\n" + e.Value;
        }

        private void OnToolValueChange(object _, KRLInt e)
        {
            toolNo.text = "Tool\n" + e.Value;
        }
    }
}
