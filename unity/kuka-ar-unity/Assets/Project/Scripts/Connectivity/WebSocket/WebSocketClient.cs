using System.Collections.Generic;
using Connectivity.Parsing;
using Connectivity.Parsing.OutputJson;
using Newtonsoft.Json;
using Project.Scripts.Utils;
using UnityEngine;
using WebSocketSharp;

namespace Connectivity
{
    public class WebSocketClient : MonoBehaviour
    {
        [SerializeField] private string serverIpAddress = "192.168.18.20:8080/kuka-variables";
        [SerializeField] private GameObject trackedRobotsHandler;
        [SerializeField] private GameObject testConnectionController;
        private TrackedRobotsHandler trackedRobotsHandlerScript;
        private HashSet<string> openConnections; 
        private WebSocket ws;
        private ConnectionTestController controller;
        private static JsonSerializerSettings settings;

        private void Awake()
        {
            trackedRobotsHandlerScript = 
                trackedRobotsHandler.GetComponent<TrackedRobotsHandler>();
            controller = testConnectionController.GetComponent<ConnectionTestController>();
        }

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
            InitializeWebsocket();
        }

        private void InitializeWebsocket()
        {
            ws = new WebSocket(serverIpAddress);

            ws.OnMessage += (_, e) => OnWebsocketMessage(e);
            
            ws.Connect();
        }

        private void OnWebsocketMessage(MessageEventArgs e)
        {
            var outputFrame = JsonConvert.DeserializeObject<OutputWithErrors>(e.Data, settings);
            trackedRobotsHandlerScript.ReceivePackageFromWebsocket(outputFrame);
            DebugLogger.Instance().AddLog("Received message ");
        }

        private void Update()
        {
            if (ws == null)
            {
                return;
            }

            // if (controller.FirstRobotConnected && !openConnections.Contains(".50"))
            // {
            //     ws.Send("{ \"host\": \"192.168.1.50\", \"var\": \"BASE\" }");
            //     openConnections.Add(".50");
            // }
            // if (controller.SecondRobotConnected && !openConnections.Contains(".51"))
            // {
            //     ws.Send("{ \"host\": \"192.168.1.51\", \"var\": \"BASE\" }");
            //     openConnections.Add(".51");
            // }
            // if (controller.ThirdRobotConnected && !openConnections.Contains(".52"))
            // {
            //     ws.Send("{ \"host\": \"192.168.1.52\", \"var\": \"BASE\" }");
            //     openConnections.Add(".52");
            // }
        }

        private void OnApplicationQuit()
        {
            ws.Close();
        }
    }
}
