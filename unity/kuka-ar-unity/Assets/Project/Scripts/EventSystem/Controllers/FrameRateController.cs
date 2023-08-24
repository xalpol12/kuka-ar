using System.Collections;
using System.Threading;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers
{
    public class FrameRateController : MonoBehaviour
    {
        public static FrameRateController frameRate; 
    
        [Header("Frame settings")] 
        private const int MaxRate = 9999;
        internal float targetFrameRate = 60.0f;
        private float currentFrameTime;

        private void Awake()
        {
            frameRate = this;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = MaxRate;
            currentFrameTime = Time.realtimeSinceStartup;
            StartCoroutine(nameof(WaitForNextFrame));
        }

        private IEnumerator WaitForNextFrame()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                currentFrameTime += 1.0f / targetFrameRate;
                var t = Time.realtimeSinceStartup;
                var sleepTime = currentFrameTime - t - 0.01f;
            
                if (sleepTime > 0)
                {
                    Thread.Sleep((int)(sleepTime * 1000));

                    while (t < currentFrameTime)
                    {
                        t = Time.realtimeSinceStartup;
                    }
                }
            }
        }
    }
}
