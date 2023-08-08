using System.Collections.Generic;
using System.Linq;
using Connectivity.Parsing;
using Connectivity.Parsing.OutputJson;
using Newtonsoft.Json;
using Project.Scripts.EventSystem;
using Project.Scripts.Utils;
using UnityEngine;
using WebSocketSharp;

namespace Connectivity
{
    public class WebSocketClient : MonoBehaviour
    {
        [SerializeField] private GameObject trackedRobotHandler;
        [SerializeField] private GameObject testConnectionController;
        private HashSet<string> openConnections; 
        private WebSocket ws;
        private ConnectionTestController controller;
        private static JsonSerializerSettings settings;

        private void Start()
        {
            settings = new()
            {
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter>
                {
                    new KRLValueJsonConverter()
                }
            };

            openConnections = new HashSet<string>();
        
            var trackedRobotHandlerScript = 
                trackedRobotHandler.GetComponent<TrackedRobotsHandler>();

            controller = testConnectionController.GetComponent<ConnectionTestController>();

            ws = new WebSocket("ws://192.168.18.20:9090/kuka-variables");
            
            ws.OnMessage += (sender, e) =>
            {
                var outputFrame = JsonConvert.DeserializeObject<OutputWithErrors>(e.Data, settings);
                trackedRobotHandlerScript.ReceivePackageFromWebsocket(outputFrame);
            };
            ws.Connect();
        }

        private void Update()
        {
            if (ws == null)
            {
                return;
            }

            if (controller.FirstRobotConnected && !openConnections.Contains(".50"))
            {
                ws.Send("{ \"host\": \"192.168.1.50\", \"var\": \"BASE\" }");
                openConnections.Add(".50");
                DebugLogger.Instance().AddLog("Connection to ip .50 opened");
            }
            if (controller.SecondRobotConnected && !openConnections.Contains(".51"))
            {
                ws.Send("{ \"host\": \"192.168.1.51\", \"var\": \"BASE\" }");
                openConnections.Add(".51");
                DebugLogger.Instance().AddLog("Connection to ip .51 opened");
            }
            if (controller.ThirdRobotConnected && !openConnections.Contains(".52"))
            {
                ws.Send("{ \"host\": \"192.168.1.52\", \"var\": \"BASE\" }");
                openConnections.Add(".52");
                DebugLogger.Instance().AddLog("Connection to ip .52 opened");
            }
        }

        private void OnApplicationQuit()
        {
            ws.Close();
        }
    }
}
