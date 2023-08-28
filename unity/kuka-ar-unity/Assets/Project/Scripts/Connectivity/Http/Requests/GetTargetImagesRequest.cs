using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Project.Scripts.Connectivity.Http.Requests
{
    public class GetTargetImagesRequest : IHttpRequest<Dictionary<String, Sprite>>
    {
        private string URL => "/stickers";
        
        public async Task<Dictionary<string, Sprite>> Execute(HttpClient httpClient)
        {
            var response = await httpClient.GetAsync(httpClient.BaseAddress + URL);
            var json = await response.Content.ReadAsStringAsync();
            var img = JsonConvert.DeserializeObject<Dictionary<String, byte[]>>(json);

            var dict = new Dictionary<string, Sprite>();
            foreach (var sticker in img)
            {
                var tex = new Texture2D(1,1);
                tex.LoadImage(sticker.Value);
                var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                dict.Add(sticker.Key, sprite);
            }
            return dict;
        }
    }
}
