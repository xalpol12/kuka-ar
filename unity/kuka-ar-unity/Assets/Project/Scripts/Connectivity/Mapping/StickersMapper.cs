using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Scripts.Connectivity.Mapping
{
    public class StickersMapper : MonoBehaviour
    {
        public static StickersMapper Instance;

        private void Awake()
        {
            Instance = this;
        }
        
        public static Dictionary<string, Sprite> MapBytesToSprite(Dictionary<string, byte[]> img)
        {
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
        
        public static List<string> MapStickerToIpAddress(Dictionary<string, byte[]> stickers)
        {
            return stickers.Select(image => image.Key).ToList()
                .Select(Version.Parse)
                .OrderBy(arg => arg)
                .Select(arg => arg.ToString())
                .ToList();
        }
    }
}
