using System.Collections.Generic;
using Connectivity.Models.AggregationClasses;
using Connectivity.Parsing.OutputJson;
using Project.Scripts.Multithreading;
using Project.Scripts.Utils;
using UnityEngine;

namespace Connectivity
{
    public class TrackedRobotsHandler : MonoBehaviour
    {
        public GameObject prefab;
        [Tooltip("Minimal difference between two update values to be registered [in meters]")]
        [Range(0f, 5f)]
        public float threshold = 0.01f;
        private Dictionary<string, TrackedRobotModel> trackedRobots;
        private HashSet<string> enqueuedIps;


        void Start()
        {
            trackedRobots = new Dictionary<string, TrackedRobotModel>();
            enqueuedIps = new HashSet<string>();
        }

        public void ReceivePackageFromWebsocket(OutputWithErrors newData)
        {
            // TODO: add unwrapping exceptions from main node
            foreach (var foundIp in newData.Values.Keys)
            {
                var robotData = newData.Values[foundIp];
                UpdateTrackedPoint(foundIp, robotData);
            }
        }

        private void UpdateTrackedPoint(string entry, Dictionary<string, ValueWithError> robotData)
        {
            if (trackedRobots.TryGetValue(entry, out var point))
            {
                point.UpdateTrackedRobotVariables(robotData);
            }
            else
            {
                if (!enqueuedIps.Contains(entry))
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        trackedRobots.Add(entry, new TrackedRobotModel(
                            Instantiate(prefab, Vector3.zero, Quaternion.identity),
                            threshold));
                        DebugLogger.Instance().AddLog($"Object for ip {entry} instantiated");
                        trackedRobots[entry].UpdateTrackedRobotVariables(robotData);

                        enqueuedIps.Remove(entry);
                    });

                    enqueuedIps.Add(entry);
                }
            }
        }
        
        void Update()
        {
            foreach (var trackedRobot in trackedRobots.Values)
            {
                trackedRobot.UpdateGameObjectOrientation();
            }
        }
    }
}
