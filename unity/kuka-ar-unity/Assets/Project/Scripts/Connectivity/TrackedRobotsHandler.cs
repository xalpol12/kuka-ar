using System.Collections.Generic;
using Connectivity.Models.AggregationClasses;
using Connectivity.Parsing.OutputJson;
using Multithreading;
using UnityEngine;

namespace Connectivity
{
    public class TrackedRobotsHandler : MonoBehaviour
    {
        public GameObject prefab;
        private Dictionary<string, TrackedRobotModel> trackedPoints;
        private HashSet<string> enqueuedIps;


        void Start()
        {
            trackedPoints = new Dictionary<string, TrackedRobotModel>();
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
            if (trackedPoints.TryGetValue(entry, out var point))
            {
                point.UpdateTrackedRobotVariables(robotData);
            }
            else
            {
                if (!enqueuedIps.Contains(entry))
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        trackedPoints.Add(entry, new TrackedRobotModel(
                            Instantiate(prefab, Vector3.zero, Quaternion.identity)));
                        Debug.Log($"Object for ip {entry} instantiated");
                        trackedPoints[entry].UpdateTrackedRobotVariables(robotData);

                        enqueuedIps.Remove(entry);
                    });

                    enqueuedIps.Add(entry);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var trackedPoint in trackedPoints.Values)
            {
                trackedPoint.UpdateGameObjectOrientation();
            }
        }
    }
}
