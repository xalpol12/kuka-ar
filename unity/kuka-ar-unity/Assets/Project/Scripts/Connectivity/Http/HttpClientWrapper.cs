using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Scripts.Connectivity.Http
{
    public class HttpClientWrapper : MonoBehaviour
    {
        public static HttpClientWrapper Instance;
        private HttpClient httpClient;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://192.168.18.20:8080/kuka-variables");
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
