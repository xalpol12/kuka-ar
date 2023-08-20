using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Project.Scripts.Connectivity.RestAPI.Commands
{
    public class GetTargetImagesCommand : IRequestCommand<Dictionary<String, byte[]>>
    {
        private readonly string url;
        
        public GetTargetImagesCommand()
        {
            url = "/images";
        }
        
        public async Task<Dictionary<string, byte[]>> Execute(HttpClient httpClient)
        {
            var response = await httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<String, byte[]>>(json);
        }
    }
}
