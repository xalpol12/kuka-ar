using System;
using System.Net.Http;
using System.Threading.Tasks;
using Project.Scripts.Connectivity.WebSocket;
using UnityEngine;

namespace Project.Scripts.Connectivity.Http
{
    public class HttpClientWrapper : MonoBehaviour
    {
        private HttpClient httpClient;
        public static HttpClientWrapper Instance;

        private string baseAddress;
        public string BaseAddress
        {
            get => baseAddress;
            set {
                if (string.IsNullOrWhiteSpace(PlayerPrefs.GetString("serverIp")))
                {
                    baseAddress = value;
                }
                else
                {
                    baseAddress = PlayerPrefs.GetString("serverIp");
                    WebSocketClient.Instance.ConnectToWebsocket($"ws://{baseAddress}:8080/kuka-variables");
                }
                httpClient.BaseAddress = new Uri($"http://{baseAddress}:8080/kuka-variables");
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            httpClient = new HttpClient();
            BaseAddress = "255.255.255.255";
        }

        public async Task<TResult> ExecuteRequest<TResult>(IHttpRequest<TResult> command)
        {
            return await command.Execute(httpClient);
        }

        private void OnDestroy()
        {
            httpClient.Dispose();
        }
    }
}
