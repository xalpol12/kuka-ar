using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.EventSystem.Events;
using Project.Scripts.EventSystem.Services.Menu;
using TMPro;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class AddRobotController : MonoBehaviour
    {
        public int id;
        public GameObject bottomNav;
        public GameObject addDialog;
        [SerializeField] private GameObject saveButton;
        internal int TransformFactor;
        internal bool ShowAddDialog;
        internal bool IsSliderHold;
        internal bool IsAddRobotPressed;
        private AddNewRobotService addNewRobotService;
        private AddRobotData data;
        private HttpService httpService;
        void Start()
        {
            ShowAddDialog = false;
            TransformFactor = 3000;
            httpService = HttpService.Instance;
            addNewRobotService = AddNewRobotService.Instance;

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
            if (id == uid)
            {
                ShowAddDialog = true;
                httpService.OnClickDataReload(4);
            }
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

            if (id == uid)
            {
                if (saveButton.GetComponent<TMP_Text>().text == "Close")
                {
                    ShowAddDialog = false;
                    return;
                }
                if (!string.IsNullOrWhiteSpace(content.IpAddress) && content.IpAddress != data.IpAddress &&
                    !string.IsNullOrWhiteSpace(content.RobotCategory) && content.RobotCategory != data.RobotCategory &&
                    !string.IsNullOrWhiteSpace(content.RobotName) && content.RobotName != data.RobotName)
                {
                    ShowAddDialog = false;
                    addNewRobotService.ResetSelectState = true;
                    httpService.PostNewRobot(data);
                    httpService.OnClickDataReload(4);
                }
            }
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
