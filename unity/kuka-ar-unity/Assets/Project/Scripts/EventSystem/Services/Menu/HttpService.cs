using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Project.Scripts.Connectivity.Enums;
using Project.Scripts.Connectivity.Models;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.EventSystem.Enums;
using UnityEngine;
using UnityEngine.Networking;

public class HttpService : MonoBehaviour
{
    public int id;
    public static HttpService Instance;
    internal string ConfiguredIp;
    internal ConnectionStatus RobotConnectionStatus;
    internal List<AddRobotData> ConfiguredRobots;
    internal List<AddRobotData> Robots;
    internal List<Sprite> Stickers;
    internal List<string> CategoryNames;
    internal bool HasInternet;

    [SerializeField]
    private float connectionTimeout;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ConfiguredIp = string.IsNullOrWhiteSpace(PlayerPrefs.GetString("serverIp")) ?
            "127.0.0.1" : PlayerPrefs.GetString("serverIp");
        ConfiguredRobots = new List<AddRobotData>();
        Robots = new List<AddRobotData>();
        Stickers = new List<Sprite>();
        CategoryNames = new List<string>();
        RobotConnectionStatus = ConnectionStatus.Disconnected;
        connectionTimeout /= 1000;
        
        GetConfigured();
        GetRobots();
        GetStickers();
        MenuEvents.Event.OnClickReloadServerData += OnClickDataReload;
    }
    
    public void OnClickDataReload(int uid)
    {
        if (id != uid) return;
        GetConfigured();
        GetRobots();
        GetStickers();
    }

    internal IEnumerator PingChosenRobot(string ip)
    {
        var ping = new Ping(ip);
        var time = 0;
        while (!ping.isDone)
        {
            if (time > connectionTimeout)
            {
                break;
            }

            time -= ping.time;
            RobotConnectionStatus = ConnectionStatus.Connecting;
            yield return new WaitForSeconds(0.05f);
        }
        
        RobotConnectionStatus = time > connectionTimeout ? ConnectionStatus.Disconnected : ConnectionStatus.Connected;
    } 

    private async void GetConfigured()
    {
        var http = CreateApiRequest($"http://{ConfiguredIp}:8080/kuka-variables/configured");
        var status = http.SendWebRequest();
        
        while (!status.isDone)
        {
            await Task.Yield();
        }
        
        var data = JsonConvert
            .DeserializeObject<Dictionary<string, Dictionary<string, RobotData>>>(http.downloadHandler.text);
        
        ConfiguredRobots = data != null ? MapConfiguredResponse(data) : new List<AddRobotData>();
        CategoryNames = ConfiguredRobots.Count > 0 ? MapUniqueCategoryNames() : new List<string>();
    }

    private async void GetRobots()
    {
        var http = CreateApiRequest($"http://{ConfiguredIp}:8080/kuka-variables/robots");
        var status = http.SendWebRequest();

        while (!status.isDone)
        {
            await Task.Yield();
        }

        var data = JsonConvert.DeserializeObject<List<AddRobotData>>(http.downloadHandler.text);

        Robots = data ?? new List<AddRobotData>();
    }

    private async void GetStickers()
    {
        var http = CreateApiRequest($"http://{ConfiguredIp}:8080/kuka-variables/stickers");
        var status = http.SendWebRequest();
        
        while (!status.isDone)
        {
            await Task.Yield();
        }
        
        var data = JsonConvert.DeserializeObject<Dictionary<string, byte[]>>(http.downloadHandler.text);
        
        Stickers = data != null ? MapStickers(data) : new List<Sprite>();
    }

    public async void PostNewRobot(object body)
    {
        var http = CreateApiRequest($"http://{ConfiguredIp}:8080/kuka-variables/add", RequestType.POST, body);
        var status = http.SendWebRequest();

        while (!status.isDone)
        {
            await Task.Yield();
        }
    }

    private static UnityWebRequest CreateApiRequest(string path, RequestType type = RequestType.GET, object data = null)
    {
        var request = new UnityWebRequest(path, type.ToString());

        if (data != null)
        {
            var raw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            request.uploadHandler = new UploadHandlerRaw(raw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type","application/json");

        return request;
    }

    private List<AddRobotData> MapConfiguredResponse(Dictionary<string, Dictionary<string, RobotData>> response)
    {
        
        var list = new List<AddRobotData>();
        var i = 0;
        foreach (var group in response)
        {
            foreach (var entry in group.Value)
            {
                var robot = new AddRobotData
                {
                    RobotName = entry.Value.Name,
                    RobotCategory = group.Key,
                    IpAddress = "FAKE 192.168.100." + i,
                };
                list.Add(robot);
                i++;
            }

            i += 5;
        }
        return list;
    }

    private static List<Sprite> MapStickers(Dictionary<string, byte[]> response)
    {
        var list = new List<Sprite>();
        foreach (var sticker in response)
        {
            var tex = new Texture2D(1,1);
            tex.LoadImage(sticker.Value);
            var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            list.Add(sprite);
        }

        return list;
    }

    private List<string> MapUniqueCategoryNames()
    {
        var list = new List<string>();
        foreach (var category in ConfiguredRobots)
        {
            if (!list.Contains(category.RobotCategory))
            {
                list.Add(category.RobotCategory);
            }
        }

        return list;
    }
}
