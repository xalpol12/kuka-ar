using System.Collections;
using Project.Scripts.Connectivity.Extensions;
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
            var newRobotsTask = http.ExecuteRequest(new GetSavedRobotsRequest());
            while (!newRobotsTask.IsCompleted)
            {
                yield return null;
            }

            popup.Try(() => storage.Robots = newRobotsTask.Result);
            yield return null;
        }

        public IEnumerator GetConfiguredRobots()
        {
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
            yield return null;
        }

        public IEnumerator GetStickers()
        {
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

            yield return null;
        }

        public IEnumerator PingRobot(string ip)
        {
            var status = http.ExecuteRequest(new PingChosenIpRequest(ip));
            while (!status.IsCompleted)
            {
                yield return null;
            }
            
            yield return null;
        }

        public IEnumerator PostRobot(Robot? robot)
        {
            if (robot != null) popup.TryWithSuccessExpected(
                () => http.ExecuteRequest(new PostNewRobotRequest(robot.Value)),
                "Robot has been added");
            yield return null;
        }
    }
}
