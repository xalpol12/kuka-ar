using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using NativeWebSocket;
using Newtonsoft.Json;
using Project.Scripts.Connectivity.Parsing;
using Project.Scripts.Connectivity.Parsing.OutputJson;
using Project.Scripts.TrackedRobots;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.Connectivity.WebSocket
{
    public class WebSocketClient : MonoBehaviour
    {
        public static WebSocketClient Instance;
        
        [SerializeField] private TrackedRobotsHandler trackedRobotsHandlerScript;
        private NativeWebSocket.WebSocket ws;
        private static JsonSerializerSettings settings;
        private ConcurrentQueue<string> messagesToSend;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter>
                {
                    new KrlValueJsonConverter()
                }
            };

            messagesToSend = new ConcurrentQueue<string>();

            trackedRobotsHandlerScript.UnsubscribeObsoleteRobotIssued += (_, s) =>
            {
                string unsubscribeQuery = $"{{ \"unsubscribeIp\": \"{s}\" }}";
                messagesToSend.Enqueue(unsubscribeQuery);
            };
        }

        public async void ConnectToWebsocket(string serverAddress)
        {
            if (ws == null)
            {
                ws = new NativeWebSocket.WebSocket(serverAddress);
            } 
            else if (ws.State == WebSocketState.Open)
            {
                await ws.Close();
                ws = new NativeWebSocket.WebSocket(serverAddress);
            }

            ws.OnMessage += OnWebsocketMessage;

            ws.OnOpen += () => 
                DebugLogger.Instance.AddLog($"Connected to ws: {serverAddress}; ");
            ws.OnError += (e) =>
                DebugLogger.Instance.AddLog($"Ws error code {e}; ");
            ws.OnClose += (_) =>
                DebugLogger.Instance.AddLog($"Closed websocket with ip {serverAddress}; ");

            await ws.Connect();
        }

        public void SendToWebSocketServer(string message)
        {
            messagesToSend.Enqueue(message);
        }

        private void OnWebsocketMessage(byte[] bytes)
        {
            var message = Encoding.UTF8.GetString(bytes);
            var outputFrame = JsonConvert.DeserializeObject<OutputWithErrors>(message, settings);
            trackedRobotsHandlerScript.ReceivePackageFromWebsocket(outputFrame);
        }

        private void Update()
        {
            if (ws == null) return;
            if (ws.State == WebSocketState.Open && 
                messagesToSend.TryDequeue(out var message))
            {
                ws.SendText(message);
            }
            
            ws.DispatchMessageQueue();
        }

        private void OnApplicationQuit()
        {
            if (ws == null) return;
            if (ws.State == WebSocketState.Open)
            {
                ws.Close();
            }
        }
    }
}
