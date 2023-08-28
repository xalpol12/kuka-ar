using Project.Scripts.Connectivity.Http;
using Project.Scripts.Connectivity.Http.Requests;
using Project.Scripts.EventSystem.Enums;
using Project.Scripts.EventSystem.Events;
using Project.Scripts.EventSystem.Services.Menu;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class BottomNavController : MonoBehaviour
    {
        public static BottomNavController Instance;
    
        public int id;
        public GameObject bottomNavPanel;
        internal SelectableStylingService StylingService;
        internal SelectableLogicService LogicService;
    
        internal bool IsCirclePressed;
        internal int TransformFactor;

        private HttpClientWrapper httpClient;
        private void Start()
        {
            StylingService = SelectableStylingService.Instance;
            LogicService = SelectableLogicService.Instance;
            
            TransformFactor = 5000;
            IsCirclePressed = false;
            PositioningService.Instance.BestFitPosition = bottomNavPanel.transform.position;
            httpClient = HttpClientWrapper.Instance;
        
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
            if (id == uid)
            {
                IsCirclePressed = true;
                httpClient.ExecuteRequest(new GetRobotConfigDataRequest());
            }
        }

        private void CirclePressed(int uid)
        {
            if (id == uid)
            {
                IsCirclePressed = false;
            }
        }
    }
}