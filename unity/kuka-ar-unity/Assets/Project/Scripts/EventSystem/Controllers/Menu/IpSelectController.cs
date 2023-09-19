using Project.Scripts.Connectivity.Enums;
using Project.Scripts.Connectivity.Http;
using Project.Scripts.EventSystem.Enums;
using Project.Scripts.EventSystem.Events;
using Project.Scripts.EventSystem.Services.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class IpSelectController : MonoBehaviour
    {
        public int id;
        public GameObject ipSelector;

        public SelectableStylingService StylingService;
        public AddNewRobotService AddNewRobotService;
        public PositioningService PositioningService;
        public WebDataStorage DataStorage;
        public ButtonType ElementClicked;
        public ButtonType PrevElementClicked;
        public GameObject HexSpinner;
        public LogicStates ShowOptionsController;
        public int TransformFactor;

        private const int GroupOffset = 1000;
        private bool showOptions;
        private Image ipField;
        private Image categoryField;
        private Image nameField;

        private void Start()
        {
            StylingService = SelectableStylingService.Instance;
            AddNewRobotService = AddNewRobotService.Instance;
            PositioningService = PositioningService.Instance;
            DataStorage = WebDataStorage.Instance;

            showOptions = false;
            HexSpinner = ipSelector.transform.Find("LoadingProgress").GetComponent<Transform>().gameObject;
            ShowOptionsController = LogicStates.Waiting;
            TransformFactor = 7500;

            var parent = ipSelector.transform.parent;
            ipField = parent.Find("IpAddress").GetComponent<Image>();
            categoryField = parent.Find("ChosenCategory").GetComponent<Image>();
            nameField = parent.Find("RobotName").GetComponent<Image>();
        
            MenuEvents.Event.OnClickIpAddress += OnClickSelectIpAddress;
        }
    
        private void OnClickSelectIpAddress(int uid)
        {
            if (!showOptions)
            {
                PrevElementClicked = ElementClicked;
                switch (uid % GroupOffset)
                {
                    case 0:
                        ElementClicked = ButtonType.IpAddress;
                        ipField.sprite = StylingService.DefaultInputField;
                        break;
                    case 1:
                        ElementClicked = ButtonType.Category;
                        categoryField.sprite = StylingService.DefaultInputField;
                        break;
                    case 2:
                        ElementClicked = ButtonType.RobotName;
                        nameField.sprite = StylingService.DefaultInputField;
                        break;
                }
            }

            uid /= GroupOffset;
            if (id != uid) return;
            showOptions = !showOptions;
            ShowOptionsController = showOptions ? LogicStates.Running : LogicStates.Hiding;
        }
    }
}
