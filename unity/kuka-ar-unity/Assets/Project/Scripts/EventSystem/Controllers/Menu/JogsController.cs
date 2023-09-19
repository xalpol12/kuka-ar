using Project.Scripts.Connectivity.Models.KRLValues;
using Project.Scripts.EventSystem.Enums;
using Project.Scripts.EventSystem.Events;
using Project.Scripts.EventSystem.Services.Menu;
using Project.Scripts.TrackedRobots;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class JogsController : MonoBehaviour
    {
        public int id;
        [Range(0f,200f)] public float transformFactor;
        public GameObject jogs;
        public bool ShowJogs;
        public bool UpdateJogs;
        public JogsControlService Service;
        public LogicStates JogsTrigger;
        public KRLJoints Joints;

        [SerializeField] private GameObject gameObjectRobotHandler;
        private TrackedRobotsHandler robotsHandler;

        private void Awake()
        {
            robotsHandler = gameObjectRobotHandler.GetComponent<TrackedRobotsHandler>();
        }

        private void Start()
        {
            Service = JogsControlService.Instance;
        
            ShowJogs = false;
            UpdateJogs = false;
            JogsTrigger = LogicStates.Waiting;
            Joints = new KRLJoints();
        
            MenuEvents.Event.OnClickJog += OnClickJog;
            robotsHandler.ActiveJointsUpdated += OnJointUpdate;
        }

        private void OnClickJog(int gui)
        {
            if (id != gui) return;
            ShowJogs = !ShowJogs;
            JogsTrigger = ShowJogs ? LogicStates.Running : LogicStates.Hiding;
        }

        private void OnJointUpdate(object sender, KRLJoints e)
        {
            Joints = e;
            UpdateJogs = true;
        }
    }
}