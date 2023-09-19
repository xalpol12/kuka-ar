using System.Collections.Generic;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.EventSystem.Enums;
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

        public readonly int ConnectionTimeOut = 1000;
        public List<Robot> ConfiguredRobots { get; set; } = new();
        public List<Robot> Robots { get; set; } = new();
        public List<string> AvailableIps { get; set; } = new();
        public Dictionary<string, Sprite> Stickers { get; set; } = new();
        public List<string> CategoryNames { get; set; } = new();
        public ConnectionStatus RobotConnectionStatus { get; set; } = ConnectionStatus.Disconnected;
        public bool IsAfterRobotSave { get; set; } = false;

        public Dictionary<string, bool> LoadingSpinner { get; set; } = new()
        {
            { "GetRobots", false },
            { "GetConfigured", false },
            { "GetStickers", false },
            { "PostNewRobot", false },
            { "UpdateRobot", false },
        };
    }
}
