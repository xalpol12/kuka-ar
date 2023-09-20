using UnityEngine;

namespace Project.Scripts.Connectivity.Extensions
{
    public class NotificationAssetWatcher : MonoBehaviour
    {
        public static NotificationAssetWatcher Watcher;
        public Sprite Wifi;
        public Sprite NoWifi;
        public Sprite AddedSuccess;
        public Sprite EditSuccess;
        public Sprite AddedFailed;

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
