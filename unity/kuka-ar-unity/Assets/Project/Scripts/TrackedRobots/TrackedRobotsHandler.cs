using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Connectivity.Enums;
using Project.Scripts.Connectivity.ExceptionHandling;
using Project.Scripts.Connectivity.Extensions.Overriders;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.Connectivity.Models.KRLValues;
using Project.Scripts.Connectivity.Parsing.OutputJson;
using Project.Scripts.EventSystem.Services.Menu;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Project.Scripts.TrackedRobots
{
    public class TrackedRobotsHandler : MonoBehaviour
    {
        [Tooltip("Prefab to be displayed as a robot's base and tool representation")]
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
        public event EventHandler RobotConnectionReset;
        public event EventHandler<string> UnsubscribeObsoleteRobotIssued;

        private string selectedRobotIP;
        private TrackedRobotModel currentlyTrackedRobot;
        private Dictionary<string, List<Renderer>> objectRenderers;

        private void Start()
        {
            objectRenderers = new()
            {
                { "base", new List<Renderer>() },
                { "tool", new List<Renderer>() }
            };
        }

        public void ChangeSelectedRobot(string robotIP)
        {
            if (robotIP == selectedRobotIP) return;
            if (selectedRobotIP != null)
            {
                OnUnsubscribeObsoleteRobot(selectedRobotIP);
                DestroyPrefab();
            }
            OnRobotConnectionReset();
            selectedRobotIP = robotIP;
            LabelOverride.Label.OverrideStatusLabel(ConnectionStatus.Connecting.ToString());
        }

        public void ResetConnectedRobot()
        {
            if (selectedRobotIP != null)
            {
                OnUnsubscribeObsoleteRobot(selectedRobotIP);
                DestroyPrefab();
                selectedRobotIP = null;
            }
            OnRobotConnectionReset();
        }

        public void SwitchBaseGameObject(bool value)
        {
            if (objectRenderers.TryGetValue("base", out var baseRenderers))
            {
                foreach (var partialBaseRenderer in baseRenderers)
                {
                    partialBaseRenderer.enabled = value;
                }
            }
        }

        public void SwitchToolGameObject(bool value)
        {
            if (objectRenderers.TryGetValue("tool", out var toolRenderers))
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
                LabelOverride.Label.OverrideStatusLabel(ConnectionStatus.Disconnected.ToString());
            }

            if (newData.Exception.HasValue)
            {
                GlobalExceptionStorage.Instance.AddException(newData.Exception.Value);
            }
        }

        public void InstantiateTrackedRobot(string ipAddress, ARAnchor anchor)
        {
            if (ipAddress == selectedRobotIP)
            {
                #if !UNITY_EDITOR || !UNITY_EDITOR_WIN
                    StartCoroutine(InstantiateNewRobot(anchor));
                #endif
            }
        }

        private void DestroyPrefab()
        {
            if (currentlyTrackedRobot == null) return;
            currentlyTrackedRobot.JointsValueUpdated -= OnJointsValueUpdated;
            currentlyTrackedRobot.BaseValueUpdated -= OnBaseValueUpdated;
            currentlyTrackedRobot.ToolValueUpdated -= OnToolValueUpdated;
            currentlyTrackedRobot = null;
            foreach (var objectsRenderers in objectRenderers.Values)
            {
                objectsRenderers.Clear();
            }
        }

        private void UpdateTrackedPoint(IReadOnlyDictionary<string, ValueWithError> robotData)
        {
            if (currentlyTrackedRobot == null) return;
            LabelOverride.Label.OverrideStatusLabel(ConnectionStatus.Connected.ToString());
            currentlyTrackedRobot.UpdateTrackedRobotVariables(robotData);
        }

        private IEnumerator InstantiateNewRobot(ARAnchor anchor)
        {
            var basePoint = anchor.transform;
            bool isInstantiated = false;
            while (!isInstantiated)
            {
                yield return null;
                var position = basePoint.position;
                var rotation = basePoint.rotation;
                var baseObject = Instantiate(prefab, position, rotation);
                var toolObject = Instantiate(prefab, position, rotation);

                objectRenderers["base"].AddRange(baseObject.GetComponentsInChildren<Renderer>());
                objectRenderers["tool"].AddRange(toolObject.GetComponentsInChildren<Renderer>());

                baseObject.transform.SetParent(anchor.transform);
                toolObject.transform.SetParent(baseObject.transform);

                currentlyTrackedRobot = new TrackedRobotModel(baseObject, toolObject,
                    positionThreshold,
                    rotationThreshold);

                currentlyTrackedRobot.JointsValueUpdated += OnJointsValueUpdated;
                currentlyTrackedRobot.BaseValueUpdated += OnBaseValueUpdated;
                currentlyTrackedRobot.ToolValueUpdated += OnToolValueUpdated;
        
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

        private void OnRobotConnectionReset()
        {
            RobotConnectionReset?.Invoke(this, EventArgs.Empty);
        }

        private void OnUnsubscribeObsoleteRobot(string e)
        {
            UnsubscribeObsoleteRobotIssued?.Invoke(this, e);
        }
    }
}
