using System;
using Project.Scripts.Connectivity.Http;
using Project.Scripts.Connectivity.Http.Requests;
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
        [Tooltip("Add new robot controller ID")]
        public int id;
        
        [Tooltip("Bottom navigation component reference")]
        public GameObject bottomNav;
        
        [Tooltip("Self reference to prevent behavior execution before component init")]
        public GameObject addDialog;
        
        [SerializeField]
        [Tooltip("Save button reference - error fix")]
        private GameObject saveButton;
        
        [Tooltip("Animation speed")]
        public int transformFactor;
        
        [NonSerialized] public bool IsSliderHold;
        [NonSerialized] public LogicStates DialogState;
        [NonSerialized] public AddNewRobotService AddNewRobotService;

        internal bool IsAddRobotPressed;
        
        private WebDataStorage webDataStorage;
        private Robot data;
        private SelectableStylingService stylingService;
        private SelectableLogicService logicService;
        private Image ipImage;
        private Image categoryImage;
        private Image nameImage;
        private void Start()
        {
            transformFactor = 3000;
            DialogState = LogicStates.Waiting;
            
            AddNewRobotService = AddNewRobotService.Instance;
            stylingService = SelectableStylingService.Instance;
            webDataStorage = WebDataStorage.Instance;
        
            ipImage = addDialog.transform.Find("IpAddress").GetComponent<Image>();
            categoryImage = addDialog.transform.Find("ChosenCategory").GetComponent<Image>();
            nameImage = addDialog.transform.Find("RobotName").GetComponent<Image>();
        
            data = new Robot
            {
                IpAddress = "IP Address",
                Category = "Category",
                Name = "Name"
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
            ServerInvoker.Invoker.GetFullData();
        }

        private void OnSave(int uid)
        {
            var content = new Robot
            {
                IpAddress = addDialog.transform.Find("IpAddress").GetComponent<RectTransform>().gameObject.transform
                    .Find("Label").GetComponent<TMP_Text>().text,
                Category = addDialog.transform.Find("ChosenCategory").GetComponent<RectTransform>().gameObject
                    .transform
                    .Find("CategoryLabel").GetComponent<TMP_Text>().text,
                Name = addDialog.transform.Find("RobotName").GetComponent<RectTransform>().gameObject.transform
                    .Find("NameLabel").GetComponent<TMP_Text>().text
            };

            if (id != uid) return;
            if (saveButton.GetComponent<TMP_Text>().text == "Close")
            {
                DialogState = LogicStates.Hiding;
                return;
            }

            if (string.IsNullOrWhiteSpace(content.IpAddress) || content.IpAddress == data.IpAddress ||
                string.IsNullOrWhiteSpace(content.Category) || content.Category == data.Category ||
                string.IsNullOrWhiteSpace(content.Name) || content.Name == data.Name)
            {
                if (string.IsNullOrWhiteSpace(content.IpAddress) || content.IpAddress == data.IpAddress)
                {
                    ipImage.sprite = stylingService.InvalidSelectable;
                }

                if (string.IsNullOrWhiteSpace(content.Category) || content.Category == data.Category)
                {
                    categoryImage.sprite = stylingService.InvalidSelectable;
                }

                if (string.IsNullOrWhiteSpace(content.Name) || content.Name == data.Name)
                {
                    nameImage.sprite = stylingService.InvalidSelectable;
                }
            
                return;
            }
            DialogState = LogicStates.Hiding;
            AddNewRobotService.ResetSelectState = true;
            StartCoroutine(ServerInvoker.Invoker.PostRobot(content));
            webDataStorage.IsAfterRobotSave = true;
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
