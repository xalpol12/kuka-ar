using Project.Scripts.EventSystem.Enums;
using Project.Scripts.EventSystem.Events;
using Project.Scripts.EventSystem.Services.Menu;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class JogsController : MonoBehaviour
    {
        public int id;
        [Range(0f,200f)] public float transformFactor;
        public GameObject jogs;
        internal bool ShowJogs;
        internal JogsControlService Service;
        internal LogicStates JogsTrigger;

        private void Start()
        {
            Service = JogsControlService.Instance;
        
            ShowJogs = false;
            JogsTrigger = LogicStates.Waiting;
        
            MenuEvents.Event.OnClickJog += OnClickJog;
        }

        private void OnClickJog(int gui)
        {
            if (id != gui) return;
            ShowJogs = !ShowJogs;
            JogsTrigger = ShowJogs ? LogicStates.Running : LogicStates.Hiding;
        }
    }
}