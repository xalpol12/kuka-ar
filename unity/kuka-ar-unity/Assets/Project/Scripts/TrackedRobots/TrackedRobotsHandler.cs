using System;
using System.Collections.Generic;
using Project.Scripts.Connectivity.ExceptionHandling;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.Connectivity.Models.KRLValues;
using Project.Scripts.Connectivity.Parsing.OutputJson;
using Project.Scripts.Multithreading;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.TrackedRobots
{
    public class TrackedRobotsHandler : MonoBehaviour
    {
        [Tooltip("Prefab to be displayed as a robot's base and tcp representation")]
        public GameObject prefab;
        
        [Tooltip("Minimal difference between two position update values to be registered [in meters]")]
        [Range(0f, 5f)]
        public float positionThreshold = 0.01f;
        
        [Tooltip("Minimal difference between two rotation update values to be registered [in degrees]")]
        [Range(0f, 360f)]
        public float rotationThreshold = 1f;

        public event EventHandler<KRLJoints> ActiveJointsUpdated;
        public event EventHandler<bool> RobotConnectionStatusConnected; 

        private string selectedRobotIP;
        private TrackedRobotModel currentlyTrackedRobot;

        public void ChangeSelectedRobotIP(string robotIP)
        {
            if (robotIP == selectedRobotIP) return;
            if (selectedRobotIP != null)
            {
                UnsubscribeFromWebsocket();
                DestroyPrefab();
            }
            selectedRobotIP = robotIP;
            OnRobotConnectionStatusConnected(this, false);
        }

        private void UnsubscribeFromWebsocket()
        {
            //TODO: implement method
        }

        private void DestroyPrefab()
        {
            currentlyTrackedRobot = null;
        }

        public void ReceivePackageFromWebsocket(OutputWithErrors newData)
        {
            foreach (var foundIp in newData.Values.Keys)
            {
                if (foundIp != selectedRobotIP) continue;
                var robotData = newData.Values[foundIp];
                UpdateTrackedPoint(robotData);
            }

            if (newData.Exception.HasValue)
            {
                GlobalExceptionStorage.Instance.AddException(newData.Exception.Value);
            }
        }

        private void UpdateTrackedPoint(Dictionary<string, ValueWithError> robotData)
        {
            if (currentlyTrackedRobot == null) return; 
            currentlyTrackedRobot.UpdateTrackedRobotVariables(robotData);
        }

        private void OnJointsValueUpdated(object sender, KRLJoints e)
        {
            ActiveJointsUpdated?.Invoke(this, e);
        }

        private void OnRobotConnectionStatusConnected(object sender, bool e)
        {
            RobotConnectionStatusConnected?.Invoke(this, e);
        }
        
        public void InstantiateTrackedRobot(string ipAddress, Transform basePoint)
        {
            if (ipAddress == selectedRobotIP)
            {
                UnityMainThreadDispatcher.Instance.Enqueue(() =>
                {
                    var position = basePoint.position;
                    var rotation = basePoint.rotation;
                    currentlyTrackedRobot = new TrackedRobotModel(
                        Instantiate(prefab, position, rotation),
                        Instantiate(prefab, position, rotation),
                        positionThreshold,
                        rotationThreshold);
                
                    DebugLogger.Instance.AddLog($"Object for ip {ipAddress} instantiated; ");

                    currentlyTrackedRobot.JointsValueUpdated += OnJointsValueUpdated;
                    OnRobotConnectionStatusConnected(this, true);
                });
            }
        }

        private void Update()
        {
            currentlyTrackedRobot?.UpdateGameObjects();
        }
    }
}
