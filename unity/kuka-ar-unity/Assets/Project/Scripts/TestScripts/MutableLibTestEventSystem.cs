using System;
using UnityEngine;

namespace Project.Scripts.TestScripts
{
    public class MutableLibTestEventSystem : MonoBehaviour
    {
        public static MutableLibTestEventSystem Current;

        private void Awake()
        {
            Current = this;
        }

        public event Action OnPressDownloadImage;

        public void DownloadImage()
        {
            OnPressDownloadImage?.Invoke();
        }
    }
}
