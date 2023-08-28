using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using UnityEngine;

namespace Project.Scripts.Connectivity.Http
{
    public class HttpClientWrapper : MonoBehaviour
    {
        private HttpClient httpClient;
        public static HttpClientWrapper Instance;
        private WebDataStorage webDataStorage;

        public string BaseAddress {
            get => BaseAddress;
            set
            {
                string ipAddress = string.IsNullOrWhiteSpace(PlayerPrefs.GetString("serverIp")) ?
                    value : PlayerPrefs.GetString("serverIp");
                httpClient.BaseAddress = new Uri($"http://{ipAddress}:8080/kuka-variables");
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            httpClient = new HttpClient();
            webDataStorage = WebDataStorage.Instance;
        }

        public async Task<TResult> ExecuteRequest<TResult>(IHttpRequest<TResult> command)
        {
            var response = await command.Execute(httpClient);
            WebStorageAssigmentHandler(command.ToString(), response);
            return response;
        }

        private void WebStorageAssigmentHandler(string command,object serverData)
        {
            switch (command)
            {
                case "robots":
                    webDataStorage.Robots = (List<Robot>) serverData;
                    break;
                case "cong":
                    webDataStorage.ConfiguredRobots = (List<Robot>)serverData;
                    webDataStorage.MapUniqueCategoryNames();
                    break;
                case "img":
                    webDataStorage.Stickers = (Dictionary<string, Sprite>)serverData;
                    webDataStorage.MapIpAddresses();
                    break;
            }
        }

        private void OnDestroy()
        {
            httpClient.Dispose();
        }
    }
}
