using System.Collections.Generic;
using Connectivity.Parsing;
using Connectivity.Parsing.OutputJson;
using Newtonsoft.Json;
using UnityEngine;
using WebSocketSharp;

namespace Connectivity
{
    public class WebSocketClient : MonoBehaviour
    {
        [SerializeField]
        private GameObject trackedRobotHandler;
        private WebSocket ws;
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

            ws = new WebSocket("ws://localhost:8080/kuka-variables");

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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ws.Send("{ \"host\": \"192.168.1.50\", \"var\": \"BASE\" }");
                Debug.Log("Connected to websocket for IP .50!");
            }
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                ws.Send("{ \"host\": \"192.168.1.51\", \"var\": \"BASE\" }");
                Debug.Log("Connected to websocket for IP .51!");
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                ws.Send("{ \"host\": \"192.168.1.52\", \"var\": \"BASE\" }");
                Debug.Log("Connected to websocket for IP .52!");
            }
        }

        private void OnApplicationQuit()
        {
            ws.Close();
        }
    }
}
