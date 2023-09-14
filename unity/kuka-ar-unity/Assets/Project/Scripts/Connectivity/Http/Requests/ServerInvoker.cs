using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using Project.Scripts.Connectivity.Enums;
using Project.Scripts.Connectivity.Extensions;
using Project.Scripts.Connectivity.Mapping;
using Project.Scripts.Connectivity.Models.AggregationClasses;
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
                            return;
                        case HttpStatusCode.BadRequest:
                            StartCoroutine(UpdateRobot(robot.Value));
                            throw new InvalidOperationException();
                        case HttpStatusCode.UnprocessableEntity:
                            throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);
                    }
                }, robot.Value, RequestType.POST);
            }
            
            storage.LoadingSpinner["PostNewRobot"] = false;
            yield return null;
        }

        public IEnumerator UpdateRobot(Robot? robot)
        {
            storage.LoadingSpinner["UpdateRobot"] = true;
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
                }, robot.Value, RequestType.PUT);
            }
            storage.LoadingSpinner["UpdateRobot"] = false;
        }
    }
}
