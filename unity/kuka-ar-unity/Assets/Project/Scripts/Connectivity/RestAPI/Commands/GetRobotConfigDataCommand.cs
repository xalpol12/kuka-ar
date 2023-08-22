using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Project.Scripts.Connectivity.Models;

namespace Project.Scripts.Connectivity.RestAPI.Commands
{
    public class GetRobotConfigDataCommand : IRequestCommand<Dictionary<string, RobotData>>
    {
        private readonly string url;
        
        public GetRobotConfigDataCommand()
        {
            url = "/configured";
        }
        
        public async Task<Dictionary<string, RobotData>> Execute(HttpClient httpClient)
        {
            var response = await httpClient.GetAsync(httpClient.BaseAddress + url);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, RobotData>>(json);
        }
    }
}
