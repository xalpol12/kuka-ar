using System.Collections.Generic;
using Project.Scripts.Connectivity.Enums;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using UnityEngine;

namespace Project.Scripts.Connectivity.Http
{
    public class WebDataStorage : MonoBehaviour
    {
        public static WebDataStorage Instance;
        public int animationTimeout;
        
        private void Awake()
        {
            Instance = this;
        }

        public const int ConnectionTimeOut = 1000;
        private const int FakeItemsNumber = 3;
        
        private List<string> availableIpsList;
        private List<string> availableCategoryNamesList;
        private List<string> availableRobotsNamesList;
        public List<Robot> robots { get; set; } = new();

        public List<string> availableRobotsNames
        {
            get => availableRobotsNamesList;
            set
            {
                for (var i = 0; i < FakeItemsNumber; i++)
                {
                    value.Add("");
                }

                availableRobotsNamesList = value;
            }
        }

        public List<string> availableIps
        {
            get => availableIpsList;
            set
            {
                for (var i = 0; i < FakeItemsNumber; i++)
                {
                    value.Add("");
                }

                availableIpsList = value;
            }
        }
        public Dictionary<string, Sprite> stickers { get; set; } = new();

        public List<string> availableCategoryNames
        {
            get => availableCategoryNamesList;
            set
            {
                for (var i = 0; i < FakeItemsNumber; i++)
                {
                    value.Add("");
                }

                availableCategoryNamesList = value;
            }
        }
        public ConnectionStatus robotConnectionStatus { get; set; } = ConnectionStatus.Disconnected;
        public bool isAfterRobotSave { get; set; }

        public Dictionary<string, bool> loadingSpinner { get; } = new()
        {
            { "GetRobots", false },
            { "GetConfigured", false },
            { "GetStickers", false },
            { "PostNewRobot", false },
            { "UpdateRobot", false },
        };
    }
}
