using System.Net.Http;
using System.Threading.Tasks;
using Project.Scripts.EventSystem.Enums;
using UnityEngine;

namespace Project.Scripts.Connectivity.Http.Requests
{
    public class PingChosenIpRequest : IHttpRequest<ConnectionStatus>
    {
        private readonly WebDataStorage storage = WebDataStorage.Instance;
        private readonly string Ip;

        public PingChosenIpRequest(string ip)
        {
            Ip = ip;
        }

        public async Task<ConnectionStatus> Execute(HttpClient client)
        {
            var ping = new Ping(Ip);
            var time = 0;
            while (!ping.isDone)
            {
                if (time > WebDataStorage.ConnectionTimeOutSel)
                {
                    break;
                }

                time -= ping.time;
                storage.RobotConnectionStatus = ConnectionStatus.Connecting;
            }
        
            storage.RobotConnectionStatus = time > WebDataStorage.ConnectionTimeOutSel ?
                ConnectionStatus.Disconnected : ConnectionStatus.Connected;
            return storage.RobotConnectionStatus;
        }
    }
}