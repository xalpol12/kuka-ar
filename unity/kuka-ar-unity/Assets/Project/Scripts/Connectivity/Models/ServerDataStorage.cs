using System.Collections.Generic;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using UnityEngine;

namespace Project.Scripts.Connectivity.Models
{
    public class ServerDataStorage
    {
        public List<AddRobotData> ConfiguredRobots { get; private set; }
        public List<Sprite> Stickers { get; private set; }
        public List<string> CategoryNames { get; private set; }
    }
}