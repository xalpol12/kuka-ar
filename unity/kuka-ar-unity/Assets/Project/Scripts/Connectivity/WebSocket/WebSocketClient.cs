using System.Collections.Generic;
using Connectivity.Parsing;
using Connectivity.Parsing.OutputJson;
using Newtonsoft.Json;
using Project.Scripts.EventSystem;
using UnityEngine;
using WebSocketSharp;

namespace Connectivity
{
    public class WebSocketClient : MonoBehaviour
    {
        [SerializeField] private GameObject trackedRobotHandler;
        [SerializeField] private GameObject testConnectionController;
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

            if (controller.FirstRobotConnected)
            {
                ws.Send("{ \"host\": \"192.168.1.50\", \"var\": \"BASE\" }");
            }
            if (controller.SecondRobotConnected)
            {
                ws.Send("{ \"host\": \"192.168.1.51\", \"var\": \"BASE\" }");
            }
            if (controller.ThirdRobotConnected)
            {
                ws.Send("{ \"host\": \"192.168.1.52\", \"var\": \"BASE\" }");
            }
        }

        private void OnApplicationQuit()
        {
            ws.Close();
        }
    }
}
