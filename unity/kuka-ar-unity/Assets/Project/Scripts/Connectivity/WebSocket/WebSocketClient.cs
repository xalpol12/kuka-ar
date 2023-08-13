using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Connectivity;
using Connectivity.Parsing;
using Connectivity.Parsing.OutputJson;
using Newtonsoft.Json;
using Project.Scripts.Utils;
using UnityEngine;
using WebSocketSharp;

namespace Project.Scripts.Connectivity.WebSocket
{
    public class WebSocketClient : MonoBehaviour
    {
        [SerializeField] private TrackedRobotsHandler trackedRobotsHandlerScript;
        private WebSocketSharp.WebSocket ws;
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

        public void ConnectToWebsocket(string serverAddress)
        {
            if (ws == null)
            {
                ws = new WebSocketSharp.WebSocket(serverAddress);
            } 
            else if (ws.IsAlive)
            {
                ws.CloseAsync();
                ws = new WebSocketSharp.WebSocket(serverAddress);
            }

            ws.OnMessage += (_, e) => OnWebsocketMessage(e);
            
            ws.Connect();

            DebugLogger.Instance()
                .AddLog(ws.IsAlive ? $"Connected to ws: {serverAddress} " : "Client couldn't connect to a server ");
        }

        public void SendToWebSocketServer(string message)
        {
            messagesToSend.Enqueue(message);
        }

        private void OnWebsocketMessage(MessageEventArgs e)
        {
            var outputFrame = JsonConvert.DeserializeObject<OutputWithErrors>(e.Data, settings);
            trackedRobotsHandlerScript.ReceivePackageFromWebsocket(outputFrame);
            // DebugLogger.Instance().AddLog("Received message ");
        }

        private void Update()
        {
            if (ws == null) return;
            if (ws.IsAlive && messagesToSend.TryDequeue(out string message))
            {
                ws.Send(message);
            }
        }

        private void OnApplicationQuit()
        {
            if (ws == null) return;
            if (ws.IsAlive)
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
