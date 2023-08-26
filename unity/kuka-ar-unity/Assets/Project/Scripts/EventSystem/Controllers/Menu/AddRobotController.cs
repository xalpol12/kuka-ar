using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.EventSystem.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.EventSystem.Controllers.Menu
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
        
        ipImage = addDialog.transform.Find("IpAddress").GetComponent<Image>();
        categoryImage = addDialog.transform.Find("ChosenCategory").GetComponent<Image>();
        nameImage = addDialog.transform.Find("RobotName").GetComponent<Image>();
        
        data = new AddRobotData
        {
            ShowAddDialog = false;
            TransformFactor = 3000;
            httpService = HttpService.Instance;
            addNewRobotService = AddNewRobotService.Instance;

    private void OnClickDisplayDialog(int uid)
    {
        if (id != uid) return;
        DialogState = LogicStates.Running;
        httpService.OnClickDataReload(4);
    }

        private void OnClickDisplayDialog(int uid)
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
        httpService.PostNewRobot(data);
        httpService.OnClickDataReload(4);
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
