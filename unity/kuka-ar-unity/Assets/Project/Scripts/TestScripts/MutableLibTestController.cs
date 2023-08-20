using System;
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
            if (ImagesDownloaded == false) return;
            
            ImagesDownloaded = true;
        }
    }
}
