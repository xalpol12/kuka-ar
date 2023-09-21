using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Scripts.Connectivity.Mapping
{
    public class RobotsMapper : MonoBehaviour
    {
        public static RobotsMapper Instance;

        private void Awake()
        {
            Instance = this;
        }

        public static List<string> MapStringToIpAddress(Dictionary<string, byte[]> stickers)
        {
            return stickers.Select(image => image.Key).ToList();
        }
    }
}
