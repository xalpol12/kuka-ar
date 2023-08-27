using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.EventSystem.Enums;
using Project.Scripts.EventSystem.Events;
using Project.Scripts.EventSystem.Services.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class AddRobotController : MonoBehaviour
    {
        public int id;
        public GameObject bottomNav;
        public GameObject addDialog;
        [SerializeField] private GameObject saveButton;
    
        internal int TransformFactor;
        internal bool IsSliderHold;
        internal bool IsAddRobotPressed;
        internal LogicStates DialogState;
        internal AddNewRobotService AddNewRobotService;

        private AddRobotData data;
        private HttpService httpService;
        private SelectableStylingService stylingService;
        private SelectableLogicService logicService;
        private Image ipImage;
        private Image categoryImage;
        private Image nameImage;
        private void Start()
        {
            TransformFactor = 3000;
            DialogState = LogicStates.Waiting;
        
            httpService = HttpService.Instance;
            AddNewRobotService = AddNewRobotService.Instance;
            stylingService = SelectableStylingService.Instance;
            logicService = SelectableLogicService.Instance;
        
            ipImage = addDialog.transform.Find("IpAddress").GetComponent<Image>();
            categoryImage = addDialog.transform.Find("ChosenCategory").GetComponent<Image>();
            nameImage = addDialog.transform.Find("RobotName").GetComponent<Image>();
        
            data = new AddRobotData
            {
                IpAddress = "IP Address",
                RobotCategory = "Category",
                RobotName = "Name"
            };
        
            MenuEvents.Event.OnClickAddNewRobot += OnClickDisplayDialog;
            MenuEvents.Event.OnRobotSave += OnSave;
            MenuEvents.Event.OnDragAddNewRobot += GrabSlider;
            MenuEvents.Event.OnDropAddNewRobot += ReleaseSlider;
        }

        private void OnClickDisplayDialog(int uid)
        {
            if (id != uid) return;
            DialogState = LogicStates.Running;
        }

        private void OnSave(int uid)
        {
            var content = new AddRobotData
            {
                IpAddress = addDialog.transform.Find("IpAddress").GetComponent<RectTransform>().gameObject.transform
                    .Find("Label").GetComponent<TMP_Text>().text,
                RobotCategory = addDialog.transform.Find("ChosenCategory").GetComponent<RectTransform>().gameObject
                    .transform
                    .Find("CategoryLabel").GetComponent<TMP_Text>().text,
                RobotName = addDialog.transform.Find("RobotName").GetComponent<RectTransform>().gameObject.transform
                    .Find("NameLabel").GetComponent<TMP_Text>().text
            };

            if (id != uid) return;
            if (saveButton.GetComponent<TMP_Text>().text == "Close")
            {
                DialogState = LogicStates.Hiding;
                return;
            }

            if (string.IsNullOrWhiteSpace(content.IpAddress) || content.IpAddress == data.IpAddress ||
                string.IsNullOrWhiteSpace(content.RobotCategory) || content.RobotCategory == data.RobotCategory ||
                string.IsNullOrWhiteSpace(content.RobotName) || content.RobotName == data.RobotName)
            {
                if (string.IsNullOrWhiteSpace(content.IpAddress) || content.IpAddress == data.IpAddress)
                {
                    ipImage.sprite = stylingService.InvalidSelectable;
                }

                if (string.IsNullOrWhiteSpace(content.RobotCategory) || content.RobotCategory == data.RobotCategory)
                {
                    categoryImage.sprite = stylingService.InvalidSelectable;
                }

                if (string.IsNullOrWhiteSpace(content.RobotName) || content.RobotName == data.RobotName)
                {
                    nameImage.sprite = stylingService.InvalidSelectable;
                }
            
                return;
            }
            DialogState = LogicStates.Hiding;
            AddNewRobotService.ResetSelectState = true;
            httpService.PostNewRobot(content);
            httpService.ReloadRobots();
            logicService.IsAfterNewRobotSave = true;
        }

        private void ReleaseSlider(int uid)
        {
            if (id == uid)
            {
                IsSliderHold = false;
            }
        }

        private void GrabSlider(int uid)
        {
            if (id == uid)
            {
                IsSliderHold = true;
            }
        }
    }
}
