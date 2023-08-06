using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class RestApiClient : MonoBehaviour
{
    [SerializeField] private string url = "http://localhost:8080/kuka-variables/";
    [SerializeField] private TextMeshProUGUI requestContent;
    [SerializeField] private Button requestButton;
    [SerializeField] private Image image;
    private HttpClient restClient;
    private Dictionary<String, byte[]> images;
    private Dictionary<String, Texture2D> images2d;
    private Sprite sprite;
    private bool isReady;
    private bool isFetched;


    private void Update()
    {
        if (isFetched)
        {
            setImages();
        }
        if (isReady)
        {
            image.sprite = sprite;
        }
    }

    void Start()
    {
        isFetched = false;
        isReady = false;
        images = new Dictionary<string, byte[]>();
        restClient = new HttpClient();
        restClient.BaseAddress = new Uri(this.url);
        requestButton.onClick.AddListener(() => StartCoroutine(GetImagesCoroutine()));
    }
    
    private IEnumerator GetRobotsCoroutine()
    {
        var robots = Task.Run(GetRobots);
        while (!robots.IsCompleted)
        {
            yield return null;
        }
        requestContent.text = JsonConvert.SerializeObject(robots.Result, Formatting.Indented);
    }

    private async Task<Dictionary<string, Dictionary<string, RobotData>>> GetRobots()
    {
        var response = await restClient.GetAsync("configured");
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, RobotData>>>(json);
    }

    private IEnumerator GetImagesCoroutine()
    {
        var images = Task.Run(GetImages);
        while (!images.IsCompleted)
        {
            yield return null;
        }

        isFetched = true;
        this.images = images.Result;
    }
    private async Task<Dictionary<String, byte[]>> GetImages()
    {
        var response = await restClient.GetAsync("stickers");
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Dictionary<String, byte[]>>(json);
    }

    private void setImages()
    {
        var dict = new Dictionary<String, Texture2D>();
        foreach (var entry in images)
        {
            Texture2D tex = new Texture2D(512, 512);
            tex.LoadImage(entry.Value);
            tex.Apply();
            dict.Add(entry.Key, tex);
        }

        var temp = dict["192.168.1.50"];
        sprite = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), Vector2.zero);
        isReady = true;
        images2d = dict;
    }
    
    

    internal class RobotData
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("positionShift")]
        public Vector PositionShift { get; set; }
        [JsonProperty("rotationShift")]
        public Vector RotationShift { get; set; }
    
    }

    internal struct Vector
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    }
    
    
}


