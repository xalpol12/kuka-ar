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
        public int id;
        public int transformFactor;
        public float dropScreenHeight;
        public GameObject jogs;
        [NonSerialized] public GameObject CoordSelectMenu;
        [NonSerialized] public GameObject ConstantTopPanel;
        
        [NonSerialized] public LogicStates ConstantPanelState;
        [NonSerialized] public LogicStates CoordListState;

        [SerializeField] private GameObject robotModel;
        private TrackedRobotModel trackedRobot;
        
        private TMP_Text toolNo;
        private TMP_Text baseNo;

        private void Awake()
        {
            trackedRobot = robotModel.GetComponent<TrackedRobotModel>();
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
            baseNo.text = e.Value.ToString();
        }

        private void OnToolValueChange(object _, KRLInt e)
        {
            toolNo.text = e.Value.ToString();
        }
    }
}
