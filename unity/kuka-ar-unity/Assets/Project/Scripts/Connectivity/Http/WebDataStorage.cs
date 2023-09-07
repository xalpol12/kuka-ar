using System.Collections.Generic;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.Connectivity.Models.SimpleValues.Pairs;
using Project.Scripts.EventSystem.Enums;
using UnityEngine;

namespace Project.Scripts.Connectivity.Http
{
    public class WebDataStorage : MonoBehaviour
    {
        public static WebDataStorage Instance;
        
        private void Awake()
        {
            Instance = this;
        }

        public const int ConnectionTimeOutSel = 1000;
        public List<Robot> ConfiguredRobots { get; set; } = new List<Robot>();
        public List<Robot> Robots { get; set; } = new List<Robot>();
        public List<string> AvailableIps { get; set; } = new List<string>();
        public Dictionary<string, Sprite> Stickers { get; set; } = new Dictionary<string, Sprite>();
        public List<string> CategoryNames { get; set; } = new List<string>();
        public ConnectionStatus RobotConnectionStatus { get; set; } = ConnectionStatus.Disconnected;
        public bool IsAfterRobotSave { get; set; } = false;
        internal Robot Response;
        internal ExceptionMessagePair PostError;
    }
}
