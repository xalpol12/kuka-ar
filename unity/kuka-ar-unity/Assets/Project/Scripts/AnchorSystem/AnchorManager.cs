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
        private Dictionary<string, RobotData> robotConfigDataByIp; // contains IP : config pairs

        private HttpClientWrapper httpClientWrapper;
        private Dictionary<string, RobotData> cachedRobotConfig;

        private readonly HashSet<string> requestVariableNames = new()
        {
            "BASE_NUMBER",
            "TOOL_NUMBER",
            "BASE",
            "POSITION",
            "JOINTS"
        };
    
        private void Awake()
        {
            arAnchorManager = gameObject.GetComponent<ARAnchorManager>();
        }

        private void Start()
        {
            trackedAnchors = new Dictionary<string, ARAnchor>();
            robotConfigDataByIp = new Dictionary<string, RobotData>();
            cachedRobotConfig = new Dictionary<string, RobotData>();
            
            httpClientWrapper = HttpClientWrapper.Instance;

            trackedRobotsHandler.RobotConnectionReset += (_, _) =>
            {
                DeleteAllTrackedAnchors();
            };
        }

        public IEnumerator LoadRequiredData()
        {
            if (cachedRobotConfig.Count == 0)
            {
                yield return StartCoroutine(ExecuteLoadingConfigData());
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

            yield return new WaitUntil(() => newConfigDataTask.IsCompleted);

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

            yield return new WaitUntil(() => newSavedRobotsTask.IsCompleted);

            foreach (var robot in newSavedRobotsTask.Result)
            {
                if (!robotConfigDataByIp.ContainsKey(robot.IpAddress))
                {
                    cachedRobotConfig.TryGetValue(robot.Name, out var robotData);
                    robotConfigDataByIp.Add(robot.IpAddress, robotData);
                }
            }
        }

        public IEnumerator StartNewAnchorTracking(ARTrackedImage foundImage)
        {
            var imageTransform = foundImage.transform;
            var robotIp = foundImage.referenceImage.name;
            var configData = robotConfigDataByIp.TryGetValue(robotIp, out var value)
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

                foreach (var requestName in requestVariableNames)
                {
                    string webSocketRequest = ComposeWebSocketServerRequest(robotIp, requestName);
                    WebSocketClient.Instance.SendToWebSocketServer(webSocketRequest);
                }

                trackedRobotsHandler.InstantiateTrackedRobot(robotIp, anchor);

                isCreated = true;
            }
        }

        private ARAnchor PlaceNewAnchor(Transform imageTransform, RobotData configData)
        {
            Vector3 position = imageTransform.position + configData.PositionShift;
            Quaternion rotation = imageTransform.rotation * Quaternion.Euler(configData.RotationShift);

            return arAnchorManager.AddAnchor(new Pose(position, rotation));
        }

        private void DeleteAllTrackedAnchors()
        {
            foreach (var anchor in trackedAnchors.Values)
            {
                arAnchorManager.RemoveAnchor(anchor);
            }
            trackedAnchors.Clear();
        }

        private static string ComposeWebSocketServerRequest(string robotIp, string variable)
        {
            var request = $"{{ \"host\": \"{robotIp}\", \"var\": \"{variable}\" }}";
            return request;
        }
    }
}
