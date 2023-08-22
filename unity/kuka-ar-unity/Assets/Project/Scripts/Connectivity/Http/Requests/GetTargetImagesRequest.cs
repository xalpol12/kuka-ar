using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Project.Scripts.Connectivity.Http.Requests
{
    public class GetTargetImagesRequest : IHttpRequest<Dictionary<String, byte[]>>
    {
        private readonly string url;
        
        public GetTargetImagesRequest()
        {
            url = "/stickers";
        }
        
        public async Task<Dictionary<string, byte[]>> Execute(HttpClient httpClient)
        {
            var response = await httpClient.GetAsync(httpClient.BaseAddress + url);
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Dictionary<String, byte[]>>(json);
        }
    }
}
