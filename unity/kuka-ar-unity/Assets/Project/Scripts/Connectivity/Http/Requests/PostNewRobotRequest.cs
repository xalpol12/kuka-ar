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
        private readonly Robot robot;
        
        public PostNewRobotRequest(Robot robot)
        {
            this.robot = robot;
        }
        
        public async Task<object> Execute(HttpClient httpClient)
        {
            var stringContent = new StringContent(
                JsonConvert.SerializeObject(robot), Encoding.UTF8, "application/json"); 
            return await httpClient.PostAsync(httpClient.BaseAddress + URL, stringContent);
        }
    }
}
