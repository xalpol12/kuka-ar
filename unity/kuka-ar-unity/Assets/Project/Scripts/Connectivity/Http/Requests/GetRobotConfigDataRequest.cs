using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Project.Scripts.Connectivity.Models;

namespace Project.Scripts.Connectivity.Http.Requests
{
    public class GetRobotConfigDataRequest : IHttpRequest<Dictionary<string, Dictionary<string, RobotData>>>
    {
        private static string URL => "/configured";
        
        public async Task<Dictionary<string, Dictionary<string, RobotData>>> Execute(HttpClient httpClient)
        {
            var response = await httpClient.GetAsync(httpClient.BaseAddress + URL);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, RobotData>>>(json);
        }
    }
}
