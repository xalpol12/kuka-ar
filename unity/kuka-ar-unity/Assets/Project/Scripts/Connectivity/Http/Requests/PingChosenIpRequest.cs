using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Project.Scripts.EventSystem.Enums;

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
            var ping = new Ping();
            await Task.Run(() =>
            {
                var reply = ping.Send(Ip);

                storage.RobotConnectionStatus = reply switch
                {
                    { Status: IPStatus.Success } => ConnectionStatus.Connected,
                    { Status: IPStatus.TimedOut or IPStatus.TimeExceeded } => ConnectionStatus.Connecting,
                    _ => ConnectionStatus.Disconnected
                };
            });
            // var ping = new Ping(Ip);
            // var time = 0;
            // while (!ping.isDone)
            // {
            //     if (time > WebDataStorage.ConnectionTimeOutSel)
            //     {
            //         break;
            //     }
            //
            //     time -= ping.time;
            //     storage.RobotConnectionStatus = ConnectionStatus.Connecting;
            // }
        
            // storage.RobotConnectionStatus = time > WebDataStorage.ConnectionTimeOut ?
                // ConnectionStatus.Disconnected : ConnectionStatus.Connected;
            return storage.RobotConnectionStatus;
        }
    }
}