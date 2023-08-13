using System.Threading.Tasks;
using Newtonsoft.Json;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using UnityEngine;
using UnityEngine.Networking;

public class AddRobotController : MonoBehaviour
{
    public int id;
    public GameObject bottomNav;
    public GameObject addDialog;
    internal int TransformFactor;
    internal bool ShowAddDialog;
    internal AddRobotData Data;
    internal bool ShowLoadingSpinner;
    internal bool CanSend;
    internal bool IsSliderHold;
    internal bool IsAddRobotPressed;
    void Start()
    {
        ShowAddDialog = false;
        ShowLoadingSpinner = false;
        CanSend = false;
        TransformFactor = 3000;
        Data = new AddRobotData
        {
            IpAddress = null,
            RobotCategory = null,
            RobotName = null
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
    }

    private async void OnSave(int uid)
    {
        if (id == uid)
        {
            if (CanSend)
            {
                const string url = "http://localhost:8080/api/add";
                var www = UnityWebRequest.PostWwwForm(url, JsonConvert.SerializeObject(Data));
                
                Debug.Log(JsonConvert.SerializeObject(Data));
                www.SetRequestHeader("Content-Type", "application/json");
                var send = www.SendWebRequest();
                
                while (!send.isDone)
                {
                    ShowLoadingSpinner = true;
                    await Task.Yield();
                }
                ShowLoadingSpinner = false;
                Debug.Log("DONE");
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
