using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Project.Scripts.Connectivity.Models.AggregationClasses;

namespace Project.Scripts.Connectivity.Http.Requests
{
    public class PostNewRobotRequest : IHttpRequest<object>
    {
        private static string URL => "/add";
        private readonly Robot newRobot;
        
        public PostNewRobotRequest(Robot robot)
        {
            newRobot = robot;
        }
        
        public async Task<object> Execute(HttpClient httpClient)
        {
            var stringContent = new StringContent(
                JsonConvert.SerializeObject(newRobot), Encoding.UTF8, "application/json");
            await httpClient.PostAsync(httpClient.BaseAddress + URL, stringContent);
            return HttpStatusCode.OK;
        }
    }
}
