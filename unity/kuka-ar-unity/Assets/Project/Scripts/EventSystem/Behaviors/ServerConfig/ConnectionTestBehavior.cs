using Project.Scripts.EventSystem.Controllers.ServerConfig;
using UnityEngine;

namespace Project.Scripts.EventSystem.Behaviors.ServerConfig
{
    public class ConnectionTestBehavior : MonoBehaviour
    {
        private ConnectionTestController controller;
    
        void Start()
        {
            controller = GetComponent<ConnectionTestController>();
        }

        void Update()
        {
        
        }
    }
}
