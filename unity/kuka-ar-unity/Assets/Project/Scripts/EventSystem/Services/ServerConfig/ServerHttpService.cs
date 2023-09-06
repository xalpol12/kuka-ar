using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading.Tasks;
using Project.Scripts.EventSystem.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.EventSystem.Services.ServerConfig
{
    public class ServerHttpService : MonoBehaviour
    {
        public static ServerHttpService Instance;
        [SerializeField] private GameObject cloudComponent;
        [SerializeField] private int timeout;
        private Sprite pingSuccessIcon;
        private Sprite pingWaitIcon;
        private Sprite pingFailedIcon;
        private Image cloudIcon;
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            cloudIcon = cloudComponent.GetComponent<Image>();
            pingSuccessIcon = Resources.Load<Sprite>("Icons/cloudSuccessIcon");
            pingWaitIcon = Resources.Load<Sprite>("Icons/cloudWaiting");
            pingFailedIcon = Resources.Load<Sprite>("Icons/cloudFailedIcon");
        }

        internal IEnumerator PingOperation(string ip)
        {
            var state = PingHostAndPort(ip);
            while (!state.IsCompleted)
            {
                SwapCloud(ConnectionStatus.Connecting);
                yield return null;
            }
            
            SwapCloud(state.Result ? ConnectionStatus.Connected : ConnectionStatus.Disconnected);
            yield return null;
        }

        private void SwapCloud(ConnectionStatus pingStatus)
        {
            switch (pingStatus)
            {
                case ConnectionStatus.Connected:
                    cloudIcon.sprite = pingSuccessIcon;
                    break;
                case ConnectionStatus.Connecting:
                    cloudIcon.sprite = pingWaitIcon;
                    break;
                case ConnectionStatus.Disconnected:
                    cloudIcon.sprite = pingFailedIcon;
                    break;
            }
        }
        
        private async Task<bool> PingHostAndPort(string host)
        {
            using var tcpClient = new TcpClient();
            var connectTask = tcpClient.ConnectAsync(host, 8080);
            var timeoutTask = Task.Delay(timeout);
                
            var completedTask = await Task.WhenAny(connectTask, timeoutTask);
            try
            {
                await completedTask;
            }
            catch (Exception)
            {
                return false;
            }

            if (timeoutTask.IsCompleted)
            {
                return false;
            }

            return connectTask.Status == TaskStatus.RanToCompletion && tcpClient.Connected;
        }
    }
}
