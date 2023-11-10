using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Project.Scripts.Connectivity.Models.AggregationClasses;

namespace Project.Scripts.Connectivity.Http.Requests
{
    public class GetSavedRobotsRequest : IHttpRequest<List<Robot>>
    {
        private static string url => "/robots";

        public async Task<List<Robot>> Execute(HttpClient httpClient)
        {
            var response = await httpClient.GetAsync(httpClient.BaseAddress + url);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Robot>>(json);
        }
    }
}
