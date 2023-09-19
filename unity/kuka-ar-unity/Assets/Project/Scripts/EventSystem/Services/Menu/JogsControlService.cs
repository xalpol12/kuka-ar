using UnityEngine;

namespace Project.Scripts.EventSystem.Services.Menu
{
    public class JogsControlService : MonoBehaviour
    {
        public static JogsControlService Instance;

        public bool IsBottomNavDocked;
        public bool IsAddRobotDialogOpen;
        private void Awake()
        {
            Instance = this;
        }
    
        private void Start()
        {
            IsBottomNavDocked = true;
            IsAddRobotDialogOpen = false;
        }
    }
}
