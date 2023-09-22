using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Connectivity.Http;
using Project.Scripts.Connectivity.Http.Requests;
using UnityEngine;

namespace Project.Scripts.Connectivity.Tests.Loadout
{
    public class FakeServerInvoker : MonoBehaviour
    {
        public static FakeServerInvoker Invoker;

        internal List<TimeSpan> Times = new List<TimeSpan>();
        
        private HttpClientWrapper http;
        private const string AbsPath =
            @"F:\kuka-ar-all\unity\kuka-ar-unity\Assets\Project\Scripts\Connectivity\Tests\Results\";
        private void Awake()
        {
            Invoker = this;
        }
        
        private void Start()
        {
            http = HttpClientWrapper.Instance;
        }
        
        public IEnumerator GetRobots(ulong i, string file = "LoadoutRobotsTests.csv")
        {
            var start = DateTime.Now;
            var newRobotsTask = http.ExecuteRequest(new GetSavedRobotsRequest());
            while (!newRobotsTask.IsCompleted)
            {
                yield return null;
            }
            
            WriteRobots(i, DateTime.Now - start, file);
            yield return null;
        }

        public IEnumerator GetConfiguredRobots(ulong i, string file = "LoadoutConfiguredRobotsTests.csv") 
        {
            var start = DateTime.Now;
            var newConfiguredRobotsTask = http.ExecuteRequest(new GetRobotConfigDataRequest());

            while (!newConfiguredRobotsTask.IsCompleted)
            {
                yield return null;
            }
            
            WriteRobots(i, DateTime.Now - start, file);
            yield return null;
        }

        public IEnumerator GetStickers(ulong i, string file = "LoadoutStickersRobotsTests.csv")
        {
            var start = DateTime.Now;
            var newStickersTask = http.ExecuteRequest(new GetTargetImagesRequest());

            while (!newStickersTask.IsCompleted)
            {
                yield return null;
            }
            
            WriteRobots(i, DateTime.Now - start, file);
            yield return null;
        }

        private static void WriteRobots(ulong index, TimeSpan timeSpan, string file)
        {
            using var sw = System.IO.File.AppendText(AbsPath + file);
            if (index == 0)
            {
                sw.WriteLine("id,time");
            }
            
            var csv = $"{index},{timeSpan.Seconds + ":" + timeSpan.Milliseconds}";
            if (string.IsNullOrWhiteSpace(csv)) return;
            sw.WriteLine(csv);
            sw.Flush();
        }
    }
}
