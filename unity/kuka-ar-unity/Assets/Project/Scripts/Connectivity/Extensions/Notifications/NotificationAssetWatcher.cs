using System;
using UnityEngine;

namespace Project.Scripts.Connectivity.Extensions.Notifications
{
    public class NotificationAssetWatcher : MonoBehaviour
    {
        public static NotificationAssetWatcher Watcher;
        [NonSerialized] public Sprite Wifi;
        [NonSerialized] public Sprite NoWifi;
        [NonSerialized] public Sprite AddedSuccess;
        [NonSerialized] public Sprite EditSuccess;
        [NonSerialized] public Sprite AddedFailed;

        private void Awake()
        {
            Watcher = this;
        }

        private void Start()
        {
            Wifi = Resources.Load<Sprite>("Icons/wifi");
            NoWifi = Resources.Load<Sprite>("Icons/noWifi");
            AddedSuccess = Resources.Load<Sprite>("Icons/success");
            EditSuccess = Resources.Load<Sprite>("Icons/pen");
            AddedFailed = Resources.Load<Sprite>("Icons/fail");
        }
    }
}
