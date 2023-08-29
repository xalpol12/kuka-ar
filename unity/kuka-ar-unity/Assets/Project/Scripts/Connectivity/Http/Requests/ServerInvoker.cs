using System.Collections;
using System.Collections.Generic;
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

        private void Start()
        {
            http = HttpClientWrapper.Instance;
            storage = WebDataStorage.Instance;
            robotsMapper = RobotsMapper.Instance;
            configuredRobotsMapper = ConfiguredRobotsMapper.Instance;
            stickersMapper = StickersMapper.Instance;

        }

        private void Awake()
        {
            Invoker = this;
        }

        public void GetFullData()
        {
            StartCoroutine(GetRobots());
            StartCoroutine(GetConfiguredRobots());
            StartCoroutine(GetStickers());
        }
        
        public IEnumerator GetRobots()
        {
            storage.Robots = http.ExecuteRequest(new GetSavedRobotsRequest()).Result;
            yield return null;
        }

        public IEnumerator GetConfiguredRobots()
        {
            var res = http.ExecuteRequest(new GetRobotConfigDataRequest()).Result;
            storage.ConfiguredRobots = configuredRobotsMapper.MapToConfiguredRobots(res);
            storage.CategoryNames = configuredRobotsMapper.MapStringsToUniqueNames(storage.ConfiguredRobots);
            yield return null;
        }

        public IEnumerator GetStickers()
        {
            var res = http.ExecuteRequest(new GetTargetImagesRequest()).Result;
            storage.Stickers = stickersMapper.MapBytesToSprite(res);

            storage.AvailableIps = robotsMapper.MapStringToIpAddress(res);
            yield return null;
        }

        public IEnumerator<ConnectionStatus> PingRobot(string ip)
        {
            yield return http.ExecuteRequest(new PingChosenIpRequest(ip)).Result;
        }

        public IEnumerator PostRobot(Robot? robot)
        {
            if (robot != null) http.ExecuteRequest(new PostNewRobotRequest(robot.Value));
            yield return null;
        }
    }
}
