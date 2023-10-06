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
            set
            {
                baseAddress = value;
                httpClient.BaseAddress = new Uri($"http://{baseAddress}:8080/kuka-variables");
            }
        }

        private void Awake()
        {
            Instance = this;
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
