using UnityEngine;

namespace Project.Scripts.Connectivity.Extensions
{
    public class NotificationAssetWatcher : MonoBehaviour
    {
        public static NotificationAssetWatcher Watcher;
        internal Sprite Wifi;
        internal Sprite NoWifi;
        internal Sprite Added;
        private void Awake()
        {
            Watcher = this;
        }

        private void Start()
        {
            Wifi = Resources.Load<Sprite>("Icons/wifi");
            NoWifi = Resources.Load<Sprite>("Icons/noWifi");
            Added = Resources.Load<Sprite>("Icons/add");
        }
    }
}
