using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Connectivity.ExceptionHandling;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.Connectivity.Models.KRLValues;
using Project.Scripts.Connectivity.Parsing.OutputJson;
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
        private List<GameObject> instantiatedObjects;

        private void Start()
        {
            instantiatedObjects = new List<GameObject>();
        }

        public void ChangeSelectedRobotIP(string robotIP)
        {
            if (robotIP == selectedRobotIP) return;
            if (selectedRobotIP != null)
            {
                UnsubscribeFromWebsocket();
                DestroyPrefab();
            }
            selectedRobotIP = robotIP;
            OnRobotConnectionStatusConnected(false);
        }

        private void UnsubscribeFromWebsocket()
        {
            //TODO: implement method
        }

        private void DestroyPrefab()
        {
            currentlyTrackedRobot.JointsValueUpdated -= OnJointsValueUpdated;
            currentlyTrackedRobot = null;
            foreach (var instantiatedObject in instantiatedObjects)
            {
                Destroy(instantiatedObject);
            }
            instantiatedObjects.Clear();
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
            DebugLogger.Instance.AddLog($"Updated joints for ip {selectedRobotIP}, j1: {e.J1.ToString()}; ");
        }

        private void OnRobotConnectionStatusConnected(bool e)
        {
            RobotConnectionStatusConnected?.Invoke(this, e);
            DebugLogger.Instance.AddLog($"Updated connection status of ip {selectedRobotIP} to: {e.ToString()}; ");
        }
        
        public void InstantiateTrackedRobot(string ipAddress, Transform basePoint)
        {
            if (ipAddress == selectedRobotIP)
            {
                #if !UNITY_EDITOR || !UNITY_EDITOR_WIN
                    StartCoroutine(InstantiateNewRobot(ipAddress, basePoint));
                #endif
                #if UNITY_EDITOR || UNITY_EDITOR_WIN
                    StartCoroutine(InstantiateNewRobot(ipAddress));
                #endif
            }
        }

        //For use in app
        private IEnumerator InstantiateNewRobot(string ipAddress, Transform basePoint)
        {
            bool isInstantiated = false;
            while (!isInstantiated)
            {
                yield return null;
                var position = basePoint.position;
                var rotation = basePoint.rotation;
                var baseObject = Instantiate(prefab, position, rotation);
                var tcpObject = Instantiate(prefab, position, rotation);
                currentlyTrackedRobot = new TrackedRobotModel(baseObject, tcpObject,
                    positionThreshold,
                    rotationThreshold);
                
                instantiatedObjects.Add(baseObject);
                instantiatedObjects.Add(tcpObject);
                
                DebugLogger.Instance.AddLog($"Object for ip {ipAddress} instantiated; ");
        
                currentlyTrackedRobot.JointsValueUpdated += OnJointsValueUpdated;
                OnRobotConnectionStatusConnected(true);
        
                isInstantiated = true;
            }
        }
        
        //For debug in editor only
        private IEnumerator InstantiateNewRobot(string ipAddress)
        {
            bool isInstantiated = false;
            while (!isInstantiated)
            {
                yield return null;
                var position = Vector3.zero;
                var rotation = Quaternion.identity;
                var baseObject = Instantiate(prefab, position, rotation);
                var tcpObject = Instantiate(prefab, position, rotation);
                currentlyTrackedRobot = new TrackedRobotModel(baseObject, tcpObject, 
                    positionThreshold, 
                    rotationThreshold);
                
                instantiatedObjects.Add(baseObject);
                instantiatedObjects.Add(tcpObject);
                
                DebugLogger.Instance.AddLog($"Object for ip {ipAddress} instantiated; ");
             
                currentlyTrackedRobot.JointsValueUpdated += OnJointsValueUpdated;
                OnRobotConnectionStatusConnected(true);
        
                isInstantiated = true;
            }
        }

        private void Update()
        {
            currentlyTrackedRobot?.UpdateGameObjects();
        }
    }
}
