using System.Collections.Generic;
using Project.Scripts.Connectivity.ExceptionHandling;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.Connectivity.Parsing.OutputJson;
using Project.Scripts.Multithreading;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.TrackedRobots
{
    public class TrackedRobotsHandler : MonoBehaviour
    {
        public GameObject prefab;
        
        [Tooltip("Minimal difference between two position update values to be registered [in meters]")]
        [Range(0f, 5f)]
        public float positionThreshold = 0.01f;
        
        [Tooltip("Minimal difference between two rotation update values to be registered [in degrees]")]
        [Range(0f, 360f)]
        public float rotationThreshold = 1f;
        
        public Dictionary<string, TrackedRobotModel> trackedRobots;
        
        private HashSet<string> enqueuedIps;

        void Start()
        {
            trackedRobots = new Dictionary<string, TrackedRobotModel>();
            enqueuedIps = new HashSet<string>();
        }

        public void ReceivePackageFromWebsocket(OutputWithErrors newData)
        {
            foreach (var foundIp in newData.Values.Keys)
            {
                var robotData = newData.Values[foundIp];
                UpdateTrackedPoint(foundIp, robotData);
            }

            if (newData.Exception.HasValue)
            {
                GlobalExceptionStorage.Instance.AddException(newData.Exception.Value);
            }
        }

        private void UpdateTrackedPoint(string entry, Dictionary<string, ValueWithError> robotData)
        {
            if (trackedRobots.TryGetValue(entry, out var point))
            {
                point.UpdateTrackedRobotVariables(robotData);
            }
            #if UNITY_EDITOR || UNITY_STANDALONE_WIN
            else
            {
                if (!enqueuedIps.Contains(entry))
                {
                    UnityMainThreadDispatcher.Instance.Enqueue(() =>
                    {
                        trackedRobots.Add(entry, new TrackedRobotModel(
                            Instantiate(prefab, Vector3.zero, Quaternion.identity), //TODO: add base offset during instantiation
                            Instantiate(prefab, Vector3.zero, Quaternion.identity),
                            positionThreshold,
                            rotationThreshold));
                        DebugLogger.Instance.AddLog($"Object for ip {entry} instantiated; ");
                        trackedRobots[entry].UpdateTrackedRobotVariables(robotData);

                        enqueuedIps.Remove(entry);
                    });

                    enqueuedIps.Add(entry);
                }
            }
            #endif
        }

        //TODO: add this method to UpdateTrackedPoint so that Unity Editor uses it
        #if !UNITY_EDITOR && !UNITY_STANDALONE_WIN //called from AnchorManager
        public void InstantiateTrackedRobot(string ipAddress, Transform basePoint)
        {
            if (!enqueuedIps.Contains(ipAddress))
            {
                UnityMainThreadDispatcher.Instance.Enqueue(() =>
                {
                    trackedRobots.Add(ipAddress, new TrackedRobotModel(
                        Instantiate(prefab, basePoint.position, basePoint.rotation),
                        positionThreshold,
                        rotationThreshold));
                    DebugLogger.Instance.AddLog($"Object for ip {ipAddress} instantiated; ");
        
                    enqueuedIps.Remove(ipAddress);
                });
        
                enqueuedIps.Add(ipAddress);
            }
        }
        #endif
        
        void Update()
        {
            foreach (var trackedRobot in trackedRobots.Values)
            {
                trackedRobot.UpdateGameObjects();
            }
        }
    }
}
