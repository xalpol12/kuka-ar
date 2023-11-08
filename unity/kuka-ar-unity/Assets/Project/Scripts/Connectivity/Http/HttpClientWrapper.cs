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

        private string baseLocalAddress;
        public string baseAddress
        {
            get => baseLocalAddress;
            set
            {
                baseLocalAddress = value;
                httpClient.BaseAddress = new Uri($"http://{baseLocalAddress}:8080/kuka-variables");
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
