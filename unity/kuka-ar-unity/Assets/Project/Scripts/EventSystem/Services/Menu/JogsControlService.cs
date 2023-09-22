using System;
using UnityEngine;

namespace Project.Scripts.EventSystem.Services.Menu
{
    public class JogsControlService : MonoBehaviour
    {
        public static JogsControlService Instance;

        [NonSerialized] public bool IsBottomNavDocked;
        [NonSerialized] public bool IsAddRobotDialogOpen;
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
