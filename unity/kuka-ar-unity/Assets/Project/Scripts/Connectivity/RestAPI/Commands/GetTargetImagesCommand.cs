using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Project.Scripts.Utils;

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
            var response = await httpClient.GetAsync(httpClient.BaseAddress.AbsolutePath + url);
            var json = await response.Content.ReadAsStringAsync();
            
            DebugLogger.Instance.AddLog(json);
            
            return JsonConvert.DeserializeObject<Dictionary<String, byte[]>>(json);
        }
    }
}
