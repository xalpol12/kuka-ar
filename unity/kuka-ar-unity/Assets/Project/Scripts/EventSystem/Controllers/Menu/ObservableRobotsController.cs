using Project.Scripts.Connectivity.Http;
using Project.Scripts.EventSystem.Services.Menu;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class ObservableRobotsController : MonoBehaviour
    {
        public GameObject parentGrid;
        internal HttpClientWrapper HttpClient;
        internal SelectableStylingService StylingService;
        internal SelectableLogicService LogicService;
        internal WebDataStorage WebDataStorage;

        private void Start()
        {
            HttpClient = HttpClientWrapper.Instance;
            StylingService = SelectableStylingService.Instance;
            LogicService = SelectableLogicService.Instance;
            WebDataStorage = WebDataStorage.Instance;
        }
    }
}
