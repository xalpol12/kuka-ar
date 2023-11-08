using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using Project.Scripts.Connectivity.Enums;
using Project.Scripts.Connectivity.Extensions.Popup;
using Project.Scripts.Connectivity.Mapping;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using UnityEngine;

namespace Project.Scripts.Connectivity.Http.Requests
{
    public class ServerInvoker : MonoBehaviour
    {
        public static ServerInvoker Invoker;
        
        private HttpClientWrapper http;
        private WebDataStorage storage;
        private Popup popup;
        
        private void Awake()
        {
            Invoker = this;
        }
        
        private void Start()
        {
            http = HttpClientWrapper.Instance;
            storage = WebDataStorage.Instance;
            popup = Popup.Window;
        }

        public void GetFullData()
        {
            StartCoroutine(GetRobots());
            StartCoroutine(GetConfiguredRobots());
            StartCoroutine(GetStickers());
        }
        
        public IEnumerator GetRobots(Action action = null)
        {
            storage.loadingSpinner["GetRobots"] = true;
            var newRobotsTask = http.ExecuteRequest(new GetSavedRobotsRequest());
            while (!newRobotsTask.IsCompleted)
            {
                yield return null;
            }
            
            popup.Try(() => storage.robots = newRobotsTask.Result);
            action?.Invoke();
            storage.loadingSpinner["GetRobots"] = false;
            yield return null;
        }

        public IEnumerator GetConfiguredRobots()
        {
            storage.loadingSpinner["GetConfigured"] = true;
            var newConfiguredRobotsTask = http.ExecuteRequest(new GetRobotConfigDataRequest());

            while (!newConfiguredRobotsTask.IsCompleted)
            {
                yield return null;
            }
            
            popup.Try(() =>
            {
                var config = ConfiguredRobotsMapper.MapToConfiguredRobots(newConfiguredRobotsTask.Result);
                storage.availableRobotsNames = config.Select(r => r.Name).ToList();
                storage.availableCategoryNames = ConfiguredRobotsMapper.MapStringsToUniqueNames(config);
            });
            storage.loadingSpinner["GetConfigured"] = false;
            yield return null;
        }

        public IEnumerator GetStickers(Action action = null)
        {
            storage.loadingSpinner["GetStickers"] = true;
            var newStickersTask = http.ExecuteRequest(new GetTargetImagesRequest());

            while (!newStickersTask.IsCompleted)
            {
                yield return null;
            }
            
            popup.Try(() =>
            {
                var stickers = newStickersTask.Result;
                storage.stickers = StickersMapper.MapBytesToSprite(stickers);
                storage.availableIps = StickersMapper.MapStickerToIpAddress(stickers);
            });
            action?.Invoke();
            storage.loadingSpinner["GetStickers"] = false;
            yield return null;
        }

        public IEnumerator PingRobot(string ip)
        {
            var status = http.ExecuteRequest(new PingChosenIpRequest(ip));
            while (!status.IsCompleted)
            {
                storage.robotConnectionStatus = ConnectionStatus.Connecting;
                yield return null;
            }

            storage.robotConnectionStatus = status.Result ? ConnectionStatus.Connected : ConnectionStatus.Disconnected;
            yield return null;
        }

        public IEnumerator PostRobot(Robot? robot)
        {
            storage.loadingSpinner["PostNewRobot"] = true;
            StartCoroutine(GetRobots());
            if (robot is not null)
            {
                var status = http.ExecuteRequest(new PostNewRobotRequest(robot.Value));

                while (!status.IsCompleted)
                {
                    yield return null;
                }
                
                popup.Try(() =>
                {
                    var response = (HttpResponseMessage)status.Result;
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            storage.robots.Add(robot.Value);
                            return;
                        case HttpStatusCode.BadRequest:
                            StartCoroutine(UpdateRobot(robot.Value));
                            throw new InvalidOperationException();
                        case HttpStatusCode.UnprocessableEntity:
                            throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);
                    }
                }, robot.Value, RequestType.Post);
            }
            
            storage.loadingSpinner["PostNewRobot"] = false;
            yield return null;
        }

        private IEnumerator UpdateRobot(Robot? robot)
        {
            storage.loadingSpinner["UpdateRobot"] = true;
            if (robot is not null)
            {
                var status = http.ExecuteRequest(new UpdateRobotRequest(robot.Value));

                while (!status.IsCompleted)
                {
                    yield return null;
                }
                
                popup.Try(() =>
                {
                    var response = (HttpResponseMessage)status.Result;
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);
                    }
                }, robot.Value, RequestType.Put);
            }
            storage.loadingSpinner["UpdateRobot"] = false;
        }
    }
}
