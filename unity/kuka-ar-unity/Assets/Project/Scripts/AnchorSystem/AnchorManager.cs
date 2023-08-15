using System.Collections;
using System.Collections.Generic;
using Connectivity;
using Project.Scripts.Connectivity.Models;
using Project.Scripts.Connectivity.WebSocket;
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
        private Dictionary<string, RobotData> robotConfigData;
    
        private void Awake()
        {
            arAnchorManager = gameObject.GetComponent<ARAnchorManager>();
        }

        private void Start()
        {
            trackedAnchors = new Dictionary<string, ARAnchor>();
            robotConfigData = new Dictionary<string, RobotData>();
        
            //TODO: delete, debug purposes
            CreateMockData();
        }

        //TODO: Implement downloading config data for each ip,
        //if not found, default: Vector3.zero
        private void CreateMockData()
        {
            RobotData newRobotConfigData = new RobotData()
            {
                Name = "kuka-test-config",
                PositionShift = new Vector3(0, -0.1f, 0),
                RotationShift = Vector3.zero
            };
            robotConfigData.Add("192.168.1.50", newRobotConfigData);
        }

        public IEnumerator CreateAnchor(ARTrackedImage foundImage)
        {
            DebugLogger.Instance().AddLog("Searching for reference points...; ");
#if !UNITY_EDITOR && !UNITY_STANDALONE_WIN
            RobotData configData = robotConfigData[foundImage.referenceImage.name];
            bool isCreated = false;
            while (!isCreated)
            {
                yield return null;
                if (foundImage.trackingState != TrackingState.Tracking) continue;
                Transform imageTransform = foundImage.transform;
                Vector3 position = imageTransform.position + configData.PositionShift;
                Quaternion rotation = imageTransform.rotation * Quaternion.Euler(configData.RotationShift);
                
                ARAnchor anchor = arAnchorManager.AddAnchor(new Pose(position, rotation)); //TODO: replace obsolete method
                DebugLogger.Instance().AddLog("AddAnchor method called; ");
                
                trackedAnchors.Add("192.168.1.50", anchor);
                
                WebSocketClient.Instance().SendToWebSocketServer(ComposeWebSocketServerRequest("192.168.1.50"));
                DebugLogger.Instance().AddLog("SendToWebSocketServer method called; ");
                
                trackedRobotsHandler.InstantiateTrackedRobot("192.168.1.50", anchor.transform);
                DebugLogger.Instance().AddLog("InstantiateTrackedRobot method called; ");
                
                isCreated = true;
            }   
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            yield return null;
#endif
            DebugLogger.Instance().AddLog("Object placed; ");
        }

        private string ComposeWebSocketServerRequest(string robotIp)
        {
            return $"{{ \"host\": \"{robotIp}\", \"var\": \"BASE\" }}";
        }
    }
}
