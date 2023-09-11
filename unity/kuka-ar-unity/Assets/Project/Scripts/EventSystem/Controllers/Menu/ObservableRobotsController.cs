using Project.Scripts.Connectivity.Http;
using Project.Scripts.EventSystem.Services.Menu;
using Project.Scripts.TrackedRobots;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class ObservableRobotsController : MonoBehaviour
    {
        public GameObject parentGrid;
        public TrackedRobotsHandler robotsHandler;
        internal SelectableStylingService StylingService;
        internal SelectableLogicService LogicService;
        internal WebDataStorage WebDataStorage;
        internal GameObject Spinner;
        private void Start()
        {
            StylingService = SelectableStylingService.Instance;
            LogicService = SelectableLogicService.Instance;
            WebDataStorage = WebDataStorage.Instance;

            Spinner = parentGrid.transform.parent.Find("LoadingProgress").GetComponent<Transform>().gameObject;
        }
    }
}
