using System;
using Project.Scripts.ImageSystem;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.TestScripts
{
    public class MutableLibTestController : MonoBehaviour
    {
        private bool ImagesDownloaded { get; set; }

        private void Start()
        {
            MutableLibTestEventSystem.Current.OnPressDownloadImage += DownloadImages;
        }

        private void DownloadImages()
        {
            if (ImagesDownloaded) return;
            DebugLogger.Instance.AddLog("Clicked download images button!; ");
            MutableImageRecognizer.Instance.LoadNewTargets();
            ImagesDownloaded = true;
        }
    }
}
