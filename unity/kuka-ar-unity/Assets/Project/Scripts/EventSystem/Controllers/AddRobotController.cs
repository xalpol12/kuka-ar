using System.Threading.Tasks;
using Newtonsoft.Json;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class AddRobotController : MonoBehaviour
{
    public int id;
    public GameObject bottomNav;
    public GameObject addDialog;
    internal int TransformFactor;
    internal bool ShowAddDialog;
    internal bool IsSliderHold;
    internal bool IsAddRobotPressed;
    private AddNewRobotService addNewRobotService;
    private AddRobotData data;
    private HttpService httpService;
    private bool isValid;
    private bool isFirst;
    void Start()
    {
        ShowAddDialog = false;
        isValid = false;
        isFirst = true;
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
            ShowAddDialog = !ShowAddDialog;
        }
        
        if (uid == 2000 && (isValid || isFirst))
        {
            ShowAddDialog = false;
            addDialog.transform.Find("IpAddress").GetComponent<RectTransform>().gameObject.transform
                .Find("Label").GetComponent<TMP_Text>().text = data.IpAddress;
            addDialog.transform.Find("ChosenCategory").GetComponent<RectTransform>().gameObject
                .transform
                .Find("CategoryLabel").GetComponent<TMP_Text>().text = data.RobotCategory;
            addDialog.transform.Find("RobotName").GetComponent<RectTransform>().gameObject.transform
                .Find("NameLabel").GetComponent<TMP_Text>().text = data.RobotName;
            addNewRobotService.ResetSelectState = true;

            if (isFirst)
            {
                isFirst = false;
            }
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
            if (!string.IsNullOrWhiteSpace(content.IpAddress) && content.IpAddress != data.IpAddress &&
                !string.IsNullOrWhiteSpace(content.RobotCategory) && content.RobotCategory != data.RobotCategory &&
                !string.IsNullOrWhiteSpace(content.RobotName) && content.RobotName != data.RobotName)
            {
                isValid = true;
                httpService.PostNewRobot(data);
            }
            else
            {
                isValid = false;
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
