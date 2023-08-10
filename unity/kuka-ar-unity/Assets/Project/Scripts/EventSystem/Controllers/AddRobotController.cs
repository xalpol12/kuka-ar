using System.Threading.Tasks;
using Newtonsoft.Json;
using Project.Scripts.Connectivity.Temp;
using UnityEngine;
using UnityEngine.Networking;

public class AddRobotController : MonoBehaviour
{
    public int id;
    public GameObject bottomNav;
    public GameObject addDialog;
    internal int TransformFactor;
    internal bool ShowAddDialog;
    internal AddRobotRequest Request;
    internal bool ShowLoadingSpinner;
    internal bool CanSend;
    internal bool IsSliderHold;
    void Start()
    {
        ShowAddDialog = false;
        ShowLoadingSpinner = false;
        CanSend = false;
        TransformFactor = 3000;
        Request = new AddRobotRequest
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
                var www = UnityWebRequest.PostWwwForm(url, JsonConvert.SerializeObject(Request));
                
                Debug.Log(JsonConvert.SerializeObject(Request));
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
