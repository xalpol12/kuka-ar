using UnityEngine;

namespace Project.Scripts.EventSystem.Services.Menu
{
    public class PositioningService : MonoBehaviour
    {
        public static PositioningService Instance;
        public readonly int PositioningError = 20;
        public Vector3 BestFitPosition;
        private void Awake()
        {
            Instance = this;
        }
    }
}
