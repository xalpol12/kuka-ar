using System.Collections;
using System.Threading;
using UnityEngine;

public class FrameRateController : MonoBehaviour
{
    [Header("Frame settings")] 
    private const int MaxRate = 9999;

    private const float targetFrameRate = 60.0f;
    private float currentFrameTime;

    private void Awake()
    {
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

            if (!(sleepTime > 0)) continue;
            Thread.Sleep((int)(sleepTime * 1000));

            while (t < currentFrameTime)
            {
                t = Time.realtimeSinceStartup;
            }
        }
    }
}
