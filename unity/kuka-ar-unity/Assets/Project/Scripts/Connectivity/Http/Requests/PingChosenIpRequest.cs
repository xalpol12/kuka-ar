using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Project.Scripts.Connectivity.Http.Requests
{
    public class PingChosenIpRequest : IHttpRequest<bool>
    {
        private readonly WebDataStorage storage = WebDataStorage.Instance;
        private readonly string ip;

        public PingChosenIpRequest(string ip)
        {
            this.ip = ip;
        }

        public async Task<bool> Execute(HttpClient client)
        {
            using var tcpClient = new TcpClient();
            var connectTask = tcpClient.ConnectAsync(ip, 8080);
            var timeoutTask = Task.Delay(storage.ConnectionTimeOut);
                
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