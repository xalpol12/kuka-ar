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

        public readonly int ConnectionTimeOutSel = 1; 
        public List<Robot> ConfiguredRobots {get; set; }
        public List<Robot> Robots { get; set; }
        public List<string> AvailableIps { get; set; }
        public Dictionary<string, Sprite> Stickers { get; set; }
        public List<string> CategoryNames { get; set; }
        public ConnectionStatus RobotConnectionStatus { get; set; } = ConnectionStatus.Disconnected;
        internal Robot Response;
        internal ExceptionMessagePair PostError;
    }
}
