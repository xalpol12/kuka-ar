using System.Collections;
using Project.Scripts.Connectivity.Extensions;
using Project.Scripts.Connectivity.Mapping;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.Connectivity.Models.SimpleValues.Pairs;
using Project.Scripts.EventSystem.Enums;
using UnityEngine;

namespace Project.Scripts.Connectivity.Http.Requests
{
    public class ServerInvoker : MonoBehaviour
    {
        public static ServerInvoker Invoker;
        
        private HttpClientWrapper http;
        private WebDataStorage storage;
        private RobotsMapper robotsMapper;
        private ConfiguredRobotsMapper configuredRobotsMapper;
        private StickersMapper stickersMapper;
        private Popup popup;
        
        private void Awake()
        {
            Invoker = this;
        }
        
        private void Start()
        {
            http = HttpClientWrapper.Instance;
            storage = WebDataStorage.Instance;
            robotsMapper = RobotsMapper.Instance;
            configuredRobotsMapper = ConfiguredRobotsMapper.Instance;
            stickersMapper = StickersMapper.Instance;
            popup = Popup.Window;
        }

        public void GetFullData()
        {
            StartCoroutine(GetRobots());
            StartCoroutine(GetConfiguredRobots());
            StartCoroutine(GetStickers());
        }
        
        public IEnumerator GetRobots()
        {
            storage.LoadingSpinner["GetRobots"] = true;
            var newRobotsTask = http.ExecuteRequest(new GetSavedRobotsRequest());
            while (!newRobotsTask.IsCompleted)
            {
                yield return null;
            }

            popup.Try(() => storage.Robots = newRobotsTask.Result);
            storage.LoadingSpinner["GetRobots"] = false;
            yield return null;
        }

        public IEnumerator GetConfiguredRobots()
        {
            storage.LoadingSpinner["GetConfigured"] = true;
            var newConfiguredRobotsTask = http.ExecuteRequest(new GetRobotConfigDataRequest());

            while (!newConfiguredRobotsTask.IsCompleted)
            {
                yield return null;
            }
            
            popup.Try(() =>
            {
                var configured = newConfiguredRobotsTask.Result;
                storage.ConfiguredRobots = configuredRobotsMapper.MapToConfiguredRobots(configured);
                storage.CategoryNames = configuredRobotsMapper.MapStringsToUniqueNames(storage.ConfiguredRobots);
            });
            storage.LoadingSpinner["GetConfigured"] = false;
            yield return null;
        }

        public IEnumerator GetStickers()
        {
            storage.LoadingSpinner["GetStickers"] = true;
            var newStickersTask = http.ExecuteRequest(new GetTargetImagesRequest());

            while (!newStickersTask.IsCompleted)
            {
                yield return null;
            }
            
            popup.Try(() =>
            {
                var stickers = newStickersTask.Result;
                storage.Stickers = stickersMapper.MapBytesToSprite(stickers);
                storage.AvailableIps = robotsMapper.MapStringToIpAddress(stickers);
            });

            storage.LoadingSpinner["GetStickers"] = false;
            yield return null;
        }

        public IEnumerator PingRobot(string ip)
        {
            var status = http.ExecuteRequest(new PingChosenIpRequest(ip));
            while (!status.IsCompleted)
            {
                storage.RobotConnectionStatus = ConnectionStatus.Connecting;
                yield return null;
            }

            storage.RobotConnectionStatus = status.Result ? ConnectionStatus.Connected : ConnectionStatus.Disconnected;
            yield return null;
        }

        public IEnumerator PostRobot(Robot? robot)
        {
            storage.LoadingSpinner["PostNewRobot"] = true;
            if (robot != null)
            {
                var status = http.ExecuteRequest(new PostNewRobotRequest(robot.Value));

                while (!status.IsCompleted)
                {
                    yield return null;
                }
                
                popup.Try(() =>
                {
                    var response = status.Result;
                    if (response is ExceptionMessagePair exception)
                    {
                        if (exception.ExceptionCode == 400)
                        {
                            Debug.Log("update robot");
                        }
                    }
                }, true);
            }
            
            storage.LoadingSpinner["PostNewRobot"] = false;
            yield return null;
        }

        public IEnumerator UpdateRobot()
        {
            yield return null;
        }
    }
}
