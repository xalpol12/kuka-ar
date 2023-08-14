using System.Collections;
using System.Collections.Generic;
using Connectivity;
using Project.Scripts.Connectivity.Models;
using Project.Scripts.Connectivity.WebSocket;
using Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

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
        OpenWebSocketConnection();
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

    private void OpenWebSocketConnection()
    {
        WebSocketClient.Instance().ConnectToWebsocket("ws://192.168.18.20:8080/kuka-variables");
    }

    public IEnumerator CreateAnchor(ARTrackedImage foundImage)
    {
        DebugLogger.Instance().AddLog("Searching for reference points... ");
        #if !UNITY_EDITOR
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
                trackedAnchors.Add("192.168.1.50", anchor);
                WebSocketClient.Instance().SendToWebSocketServer(ComposeWebSocketServerRequest("192.168.1.50"));
                trackedRobotsHandler.InstantiateTrackedRobot("192.168.1.50", anchor.transform);
                isCreated = true;
            }   
        #endif
        #if UNITY_EDITOR
            yield return null;
        #endif
        DebugLogger.Instance().AddLog("Object placed; ");
    }

    private string ComposeWebSocketServerRequest(string robotIp)
    {
        return $"{{ \"host\": \"{robotIp}\", \"var\": \"BASE\" }}";
    }
}
