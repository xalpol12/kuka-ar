using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Connectivity.Enums;
using Project.Scripts.Connectivity.ExceptionHandling;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.Connectivity.Models.KRLValues;
using Project.Scripts.Connectivity.Parsing.OutputJson;
using Project.Scripts.EventSystem.Services.Menu;
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
        public event EventHandler<KRLInt> ActiveBaseUpdated;
        public event EventHandler<KRLInt> ActiveToolUpdated;
        public event EventHandler<bool> RobotConnectionStatusConnected;
        public event EventHandler<string> UnsubscribeObsoleteRobotIssued;

        private string selectedRobotIP;
        private TrackedRobotModel currentlyTrackedRobot;
        private Dictionary<string, GameObject> instantiatedObjects;
        private Dictionary<string, List<Renderer>> instantiatedObjectRenderers;

        private void Start()
        {
            instantiatedObjects = new();
            instantiatedObjectRenderers = new()
            {
                { "base", new List<Renderer>() },
                { "tool", new List<Renderer>() }
            };
        }

        public void ChangeSelectedRobotIP(string robotIP)
        {
            if (robotIP == selectedRobotIP) return;
            if (selectedRobotIP != null)
            {
                OnUnsubscribeObsoleteRobot(selectedRobotIP);
                DestroyPrefab();
            }
            selectedRobotIP = robotIP;
            SelectableLogicService.Instance.RobotConnectionStatus = ConnectionStatus.Connecting;
            OnRobotConnectionStatusConnected(false);
        }

        public void SwitchBaseGameObject(bool value)
        {
            if (instantiatedObjectRenderers.TryGetValue("base", out var baseRenderers))
            {
                foreach (var partialBaseRenderer in baseRenderers)
                {
                    partialBaseRenderer.enabled = value;
                }
            }
        }

        public void SwitchToolGameObject(bool value)
        {
            if (instantiatedObjectRenderers.TryGetValue("tool", out var toolRenderers))
            {
                foreach (var partialToolRenderer in toolRenderers)
                {
                    partialToolRenderer.enabled = value;
                }
            }
        }

        public void ReceivePackageFromWebsocket(OutputWithErrors newData)
        {
            if (newData.Values.TryGetValue(selectedRobotIP, out var value))
            {
                UpdateTrackedPoint(value);
            }
            else
            {
                SelectableLogicService.Instance.RobotConnectionStatus = ConnectionStatus.Disconnected;
            }

            if (newData.Exception.HasValue)
            {
                GlobalExceptionStorage.Instance.AddException(newData.Exception.Value);
            }
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

        private void DestroyPrefab()
        {
            currentlyTrackedRobot.JointsValueUpdated -= OnJointsValueUpdated;
            currentlyTrackedRobot.BaseValueUpdated -= OnBaseValueUpdated;
            currentlyTrackedRobot.ToolValueUpdated -= OnToolValueUpdated;
            currentlyTrackedRobot = null;
            foreach (var objectsRenderers in instantiatedObjectRenderers.Values)
            {
                objectsRenderers.Clear();
            }
            foreach (var instantiatedObject in instantiatedObjects.Values)
            {
                Destroy(instantiatedObject);
            }
            instantiatedObjects.Clear();
        }

        private void UpdateTrackedPoint(Dictionary<string, ValueWithError> robotData)
        {
            if (currentlyTrackedRobot == null) return;
            SelectableLogicService.Instance.RobotConnectionStatus = ConnectionStatus.Connected;
            currentlyTrackedRobot.UpdateTrackedRobotVariables(robotData);
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
                var toolObject = Instantiate(prefab, position, rotation);

                instantiatedObjectRenderers["base"].AddRange(baseObject.GetComponentsInChildren<Renderer>());
                instantiatedObjectRenderers["tool"].AddRange(toolObject.GetComponentsInChildren<Renderer>());

                toolObject.transform.SetParent(baseObject.transform);

                currentlyTrackedRobot = new TrackedRobotModel(baseObject, toolObject,
                    positionThreshold,
                    rotationThreshold);

                instantiatedObjects.Add("base", baseObject);
                instantiatedObjects.Add("tool", toolObject);


                currentlyTrackedRobot.JointsValueUpdated += OnJointsValueUpdated;
                currentlyTrackedRobot.BaseValueUpdated += OnBaseValueUpdated;
                currentlyTrackedRobot.ToolValueUpdated += OnToolValueUpdated;
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
                var toolObject = Instantiate(prefab, position, rotation);

                instantiatedObjectRenderers["base"].AddRange(baseObject.GetComponentsInChildren<Renderer>());
                instantiatedObjectRenderers["tool"].AddRange(toolObject.GetComponentsInChildren<Renderer>());

                toolObject.transform.SetParent(baseObject.transform);

                currentlyTrackedRobot = new TrackedRobotModel(baseObject, toolObject,
                    positionThreshold,
                    rotationThreshold);

                instantiatedObjects.Add("base", baseObject);
                instantiatedObjects.Add("tool", toolObject);
             
                currentlyTrackedRobot.JointsValueUpdated += OnJointsValueUpdated;
                currentlyTrackedRobot.BaseValueUpdated += OnBaseValueUpdated;
                currentlyTrackedRobot.ToolValueUpdated += OnToolValueUpdated;
                OnRobotConnectionStatusConnected(true);
        
                isInstantiated = true;
            }
        }

        private void Update()
        {
            currentlyTrackedRobot?.UpdateGameObjects();
        }
        
        private void OnJointsValueUpdated(object sender, KRLJoints e)
        {
            ActiveJointsUpdated?.Invoke(this, e);
        }

        private void OnBaseValueUpdated(object sender, KRLInt e)
        {
            ActiveBaseUpdated?.Invoke(this, e);
        }

        private void OnToolValueUpdated(object sender, KRLInt e)
        {
            ActiveToolUpdated?.Invoke(this, e);
        }

        private void OnRobotConnectionStatusConnected(bool e)
        {
            RobotConnectionStatusConnected?.Invoke(this, e);
        }
        
        private void OnUnsubscribeObsoleteRobot(string e)
        {
            UnsubscribeObsoleteRobotIssued?.Invoke(this, e);
        }
    }
}
