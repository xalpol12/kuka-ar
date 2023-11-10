using System.Collections.Generic;
using System.Linq;
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

        public static List<string> MapStringsToUniqueNames(IEnumerable<Robot> names)
        {
            var list = new List<string>();
            foreach (var category in names.Where(category => !list.Contains(category.Category)))
            {
                list.Add(category.Category);
            }

            return list;
        }
        
        public static List<Robot> MapToConfiguredRobots(Dictionary<string, Dictionary<string, RobotData>> response)
        {
            return (from @group in response from entry 
                in @group.Value select new Robot { Name = entry.Value.Name, Category = @group.Key, }).ToList();
        }
    }
}
