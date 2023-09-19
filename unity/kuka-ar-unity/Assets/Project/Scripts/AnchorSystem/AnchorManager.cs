using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Project.Scripts.Connectivity.Http;
using Project.Scripts.Connectivity.Http.Requests;
using Project.Scripts.Connectivity.Models;
using Project.Scripts.Connectivity.WebSocket;
using Project.Scripts.TrackedRobots;
using Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Project.Scripts.AnchorSystem
{
    [RequireComponent(typeof(ARAnchorManager))]
    public class AnchorManager : MonoBehaviour
    {
        private ARAnchorManager arAnchorManager;
        [SerializeField] private TrackedRobotsHandler trackedRobotsHandler;
    
        private Dictionary<string, ARAnchor> trackedAnchors;
        private Dictionary<string, RobotData> robotConfigData; // contains IP : config pairs

        private HttpClientWrapper httpClientWrapper;
        private Dictionary<string, RobotData> cachedRobotConfig;
    
        private void Awake()
        {
            arAnchorManager = gameObject.GetComponent<ARAnchorManager>();
        }

        private void Start()
        {
            trackedAnchors = new Dictionary<string, ARAnchor>();
            robotConfigData = new Dictionary<string, RobotData>();
            cachedRobotConfig = new Dictionary<string, RobotData>();
            
            httpClientWrapper = HttpClientWrapper.Instance;
        }

        public IEnumerator LoadRequiredData()
        {
            if (cachedRobotConfig.Count == 0)
            {
                StartCoroutine(ExecuteLoadingConfigData());

                while (cachedRobotConfig.Count == 0)
                {
                    yield return null;
                }

                StartCoroutine(ExecuteLoadingSavedRobots());
            }
            else
            {
                StartCoroutine(ExecuteLoadingSavedRobots());
            }
        }
        
        private IEnumerator ExecuteLoadingConfigData()
        {
            var newConfigDataTask = httpClientWrapper.ExecuteRequest(new GetRobotConfigDataRequest());

            while (!newConfigDataTask.IsCompleted)
            {
                yield return null;
            }

            foreach (var category in newConfigDataTask.Result)
            {
                foreach (var subcategory in category.Value)
                {
                    cachedRobotConfig.Add(subcategory.Key, subcategory.Value);
                }
            }
        }

        private IEnumerator ExecuteLoadingSavedRobots()
        {
            var newSavedRobotsTask = httpClientWrapper.ExecuteRequest(new GetSavedRobotsRequest());

            while (!newSavedRobotsTask.IsCompleted)
            {
                yield return null;
            }

            foreach (var robot in newSavedRobotsTask.Result)
            {
                if (!robotConfigData.ContainsKey(robot.IpAddress))
                {
                    cachedRobotConfig.TryGetValue(robot.Name, out var robotData);
                    robotConfigData.Add(robot.IpAddress, robotData);
                }
            }
        }

        public IEnumerator StartNewAnchorTracking(ARTrackedImage foundImage)
        {
            DebugLogger.Instance.AddLog("Searching for reference points...; ");

            #if !UNITY_EDITOR && !UNITY_STANDALONE_WIN
            var imageTransform = foundImage.transform;
            var robotIp = foundImage.referenceImage.name;
            var configData = robotConfigData.TryGetValue(robotIp, out var value)
                ? value
                : new RobotData()
                {
                    Name = "kuka-default-config",
                    PositionShift = Vector3.zero,
                    RotationShift = Vector3.zero
                };

            bool isCreated = false;
            while (!isCreated)
            {
                yield return null;
                if (foundImage.trackingState != TrackingState.Tracking) continue;

                var anchor = PlaceNewAnchor(imageTransform, configData);
                trackedAnchors.Add(robotIp, anchor);

                WebSocketClient.Instance.SendToWebSocketServer(ComposeWebSocketServerRequest(robotIp, "BASE"));
                WebSocketClient.Instance.SendToWebSocketServer(ComposeWebSocketServerRequest(robotIp, "POSITION"));
                WebSocketClient.Instance.SendToWebSocketServer(ComposeWebSocketServerRequest(robotIp, "JOINTS"));

                trackedRobotsHandler.InstantiateTrackedRobot(robotIp, anchor.transform);

                isCreated = true;
            }
            #endif

            #if UNITY_EDITOR || UNITY_STANDALONE_WIN
            yield return null;
            #endif

            DebugLogger.Instance.AddLog("Object placed; ");
        }

        private ARAnchor PlaceNewAnchor(Transform imageTransform, RobotData configData)
        {
            Vector3 position = imageTransform.position + configData.PositionShift;
            Quaternion rotation = imageTransform.rotation * Quaternion.Euler(configData.RotationShift);
                
            return arAnchorManager.AddAnchor(new Pose(position, rotation)); //TODO: replace obsolete method
        }

        private static string ComposeWebSocketServerRequest(string robotIp, string variable)
        {
            var request = $"{{ \"host\": \"{robotIp}\", \"var\": \"{variable}\" }}";
            DebugLogger.Instance.AddLog($"{request}; ");
            return request;
        }
    }
}
