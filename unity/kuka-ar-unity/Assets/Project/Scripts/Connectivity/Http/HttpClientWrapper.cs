using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Scripts.Connectivity.Http
{
    public class HttpClientWrapper : MonoBehaviour
    {
        private HttpClient httpClient;
        public static HttpClientWrapper Instance;

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
