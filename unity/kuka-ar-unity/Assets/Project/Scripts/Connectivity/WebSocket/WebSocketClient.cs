using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        [SerializeField] private TrackedRobotsHandler trackedRobotsHandlerScript;
        private NativeWebSocket.WebSocket ws;
        private static JsonSerializerSettings settings;
        private ConcurrentQueue<string> messagesToSend;

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

            messagesToSend = new ConcurrentQueue<string>();
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

            DebugLogger.Instance.AddLog("Await ws.Connect(); ");
            await ws.Connect();
        }

        public void SendToWebSocketServer(string message)
        {
            messagesToSend.Enqueue(message);
        }

        private void OnWebsocketMessage(byte[] bytes)
        {
            String message = System.Text.Encoding.UTF8.GetString(bytes);
            var outputFrame = JsonConvert.DeserializeObject<OutputWithErrors>(message, settings);
            trackedRobotsHandlerScript.ReceivePackageFromWebsocket(outputFrame);
        }

        private void Update()
        {
            if (ws == null) return;
            if (ws.State == WebSocketState.Open && 
                messagesToSend.TryDequeue(out string message))
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

        #region Singleton logic

        private static WebSocketClient instance;

        public static WebSocketClient Instance()
        {
            if (!Exists())
            {
                throw new Exception(
                    "WebSocketClient could not find the WebSocketClient object." +
                    "Please ensure you have added the WebSocketClient Prefab to your scene.");
            }
            return instance;
        }

        private static bool Exists()
        {
            return instance != null;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }

        #endregion
    }
}
