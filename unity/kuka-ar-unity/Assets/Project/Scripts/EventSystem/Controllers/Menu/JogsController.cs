using Project.Scripts.EventSystem.Events;
using Project.Scripts.EventSystem.Services.Menu;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class JogsController : MonoBehaviour
    {
        public int id;
        public float transformFactor;
        public GameObject jogs;
        private int defaultTransformFactor;
        internal JogsControlService Service;
        internal bool ShowJogs;

        void Start()
        {
            Service = JogsControlService.Instance;
        
        
            ShowJogs = false;
            defaultTransformFactor = 10;
        
            ValueCheck();
        
            MenuEvents.Event.OnClickJog += OnClickJog;
        }

        private void OnClickJog(int gui)
        {
            if (id == gui)
            {
                ShowJogs = !ShowJogs;
            }
        }

        private void ValueCheck()
        {
            if (transformFactor is > 200f or < 0f)
            {
                transformFactor = defaultTransformFactor;
            }
        }
    }
}