using System;
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
        [Tooltip("Controller ID")]
        public int id;
        
        [Range(0f,200f)]
        [Tooltip("Animation speed")]
        public float transformFactor;
        
        [NonSerialized] public bool ShowJogs;
        [NonSerialized] public bool UpdateJogs;
        [NonSerialized] public JogsControlService Service;
        [NonSerialized] public LogicStates JogsTrigger;
        [NonSerialized] public KRLJoints Joints;

        [SerializeField]
        [Tooltip("Robot handler component")]
        private GameObject robotHandler;
        
        private TrackedRobotsHandler trackedRobotsHandler;

        private void Awake()
        {
            trackedRobotsHandler = robotHandler.GetComponent<TrackedRobotsHandler>();
        }

        private void Start()
        {
            Service = JogsControlService.Instance;
        
            ShowJogs = false;
            UpdateJogs = false;
            JogsTrigger = LogicStates.Waiting;
            Joints = new KRLJoints();
        
            MenuEvents.Event.OnClickJog += OnClickJog;
            trackedRobotsHandler.ActiveJointsUpdated += OnJointUpdate;
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