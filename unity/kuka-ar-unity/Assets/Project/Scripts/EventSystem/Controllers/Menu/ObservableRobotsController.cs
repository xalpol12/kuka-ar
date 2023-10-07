using System;
using Project.Scripts.Connectivity.Http;
using Project.Scripts.EventSystem.Services.Menu;
using Project.Scripts.TrackedRobots;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class ObservableRobotsController : MonoBehaviour
    {
        [Tooltip("Aggregation element for grid entries")]
        public GameObject parentGrid;
        
        [Tooltip("Tracked robots component reference")]
        public TrackedRobotsHandler robotsHandler;
        
        [NonSerialized] public SelectableStylingService StylingService;
        [NonSerialized] public SelectableLogicService LogicService;
        [NonSerialized] public WebDataStorage WebDataStorage;
        [NonSerialized] public GameObject Spinner;

        private void Start()
        {
            StylingService = SelectableStylingService.Instance;
            LogicService = SelectableLogicService.Instance;
            WebDataStorage = WebDataStorage.Instance;

            Spinner = parentGrid.transform.parent.Find("LoadingProgress").GetComponent<Transform>().gameObject;
        }
    }
}
