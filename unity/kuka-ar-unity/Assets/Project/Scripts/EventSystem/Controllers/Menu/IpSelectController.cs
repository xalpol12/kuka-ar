using Project.Scripts.Connectivity.Enums;
using Project.Scripts.EventSystem.Events;
using Project.Scripts.EventSystem.Services.Menu;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class IpSelectController : MonoBehaviour
    {
        public int id;
        public GameObject ipSelector;
        internal SelectableStylingService StylingService;
        internal HttpService HttpService;
        internal ButtonType ElementClicked;
        internal ButtonType PrevElementClicked;
        internal AddNewRobotService AddNewRobotService;
        internal PositioningService PositioningService;
        internal bool ShowOptions;
        internal int TransformFactor;
    
        private const int GroupOffset = 1000;

        private void Start()
        {
            HttpService = HttpService.Instance;
            StylingService = SelectableStylingService.Instance;
            AddNewRobotService = AddNewRobotService.Instance;
            PositioningService = PositioningService.Instance;
        
            ShowOptions = false;
            TransformFactor = 7500;
        
            MenuEvents.Event.OnClickIpAddress += OnClickSelectIpAddress;
        }
    
        private void OnClickSelectIpAddress(int uid)
        {
            if (!ShowOptions)
            {
                PrevElementClicked = ElementClicked;
                switch (uid % GroupOffset)
                {
                    case 0:
                        ElementClicked = ButtonType.IpAddress;
                        break;
                    case 1:
                        ElementClicked = ButtonType.Category;
                        break;
                    case 2:
                        ElementClicked = ButtonType.RobotName;
                        break;
                }
            }

            uid /= GroupOffset;
            if (id == uid)
            {
                ShowOptions = !ShowOptions;
            }
        }
    }
}
