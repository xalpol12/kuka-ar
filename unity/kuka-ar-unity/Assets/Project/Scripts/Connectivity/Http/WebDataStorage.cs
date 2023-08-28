using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.Connectivity.Models.SimpleValues.Pairs;
using Project.Scripts.EventSystem.Enums;
using UnityEngine;

namespace Project.Scripts.Connectivity.Http
{
    public class WebDataStorage : MonoBehaviour
    {
        public static WebDataStorage Instance;

        public int ConnectionTimeOutSel = 1; 
        public List<Robot> ConfiguredRobots;
        public List<Robot> Robots;
        public List<string> AvailableIps;
        public Dictionary<string, Sprite> Stickers;
        public List<string> CategoryNames;
        public ConnectionStatus RobotConnectionStatus;
        internal Robot Response;
        internal ExceptionMessagePair PostError;

        private void Awake()
        {
            Instance = this;
        }
        
        internal void MapUniqueCategoryNames()
        {
            foreach (var category in ConfiguredRobots)
            {
                if (!CategoryNames.Contains(category.Category))
                {
                    CategoryNames.Add(category.Category);
                }
            }
        }

        internal void MapIpAddresses()
        {
            AvailableIps = Stickers.Select(image => image.Key).ToList();
        }
    }
}
