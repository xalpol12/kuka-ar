using System.Collections.Generic;
using Project.Scripts.Connectivity.Models;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using UnityEngine;

namespace Project.Scripts.Connectivity.Mapping
{
    public class ConfiguredRobotsMapper : MonoBehaviour
    {
        public static ConfiguredRobotsMapper Instance;

        private void Awake()
        {
            Instance = this;
        }

        public List<string> MapStringsToUniqueNames(List<Robot> names)
        {
            var list = new List<string>();
            foreach (var category in names)
            {
                if (!list.Contains(category.Category))
                {
                    list.Add(category.Category);
                }
            }

            return list;
        }
        
        public List<Robot> MapToConfiguredRobots(Dictionary<string, Dictionary<string, RobotData>> response)
        {
            var list = new List<Robot>();
            foreach (var group in response)
            {
                foreach (var entry in group.Value)
                {
                    var robot = new Robot()
                    {
                        Name = entry.Value.Name,
                        Category = group.Key, 
                    };
                    list.Add(robot);
                }
            }
            return list;
        }
    }
}
