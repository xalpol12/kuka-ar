using UnityEngine;

namespace Project.Scripts.EventSystem.Services.Menu
{
    public class PositioningService : MonoBehaviour
    {
        public static PositioningService Instance;
        public const int PositioningError = 20;
        public Vector3 bestFitPosition;
        private void Awake()
        {
            Instance = this;
        }
    }
}
