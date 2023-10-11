using System;
using Project.Scripts.Connectivity.Http.Requests;
using Project.Scripts.EventSystem.Enums;
using Project.Scripts.EventSystem.Events;
using Project.Scripts.EventSystem.Services.Menu;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class BottomNavController : MonoBehaviour
    {
        [Tooltip("Bottom navigation controller ID")]
        public int id;
        
        [Tooltip("Bottom navigation fill height panel component reference")]
        public GameObject bottomNavPanel;

        [NonSerialized] public SelectableStylingService StylingService;
        [NonSerialized] public SelectableLogicService LogicService;

        [NonSerialized] public bool IsCirclePressed;
        [NonSerialized] public int TransformFactor;

        private void Start()
        {
            StylingService = SelectableStylingService.Instance;
            LogicService = SelectableLogicService.Instance;
            
            TransformFactor = 5000;
            IsCirclePressed = false;
            PositioningService.Instance.bestFitPosition = bottomNavPanel.transform.position;
        
            MenuEvents.Event.OnPressConstantSelectorSlider += BottomNavOnMove;
            MenuEvents.Event.OnReleaseConstantSelectorSlider += BottomNavToDockPosition;
            MenuEvents.Event.OnPointerPressCircle += CirclePress;
            MenuEvents.Event.OnPointerPressedCircle += CirclePressed;
        }

        private void BottomNavOnMove(int uid)
        {
            if (uid != id) return;
            LogicService.SliderState = LogicStates.Running;
        }

        private void BottomNavToDockPosition(int uid)
        {
            if (uid != id) return;
            LogicService.SliderState = LogicStates.Hiding;
        }

        private void CirclePress(int uid)
        {
            if (id != uid) return;
            IsCirclePressed = true;
            StartCoroutine(ServerInvoker.Invoker.GetConfiguredRobots());
        }

        private void CirclePressed(int uid)
        {
            if (uid != id) return;
            IsCirclePressed = false;
        }
    }
}