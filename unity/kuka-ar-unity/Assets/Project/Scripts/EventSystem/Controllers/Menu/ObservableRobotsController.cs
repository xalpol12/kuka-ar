using Project.Scripts.EventSystem.Services.Menu;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class ObservableRobotsController : MonoBehaviour
    {
        public GameObject parentGrid;
        internal HttpService HttpService;
        internal BottomNavController BottomNavController;
        internal SelectableStylingService StylingService;
        void Start()
        {
            HttpService = HttpService.Instance;
            StylingService = SelectableStylingService.Instance;
            BottomNavController = BottomNavController.Instance;
        
            HttpService.OnClickDataReload(4);
        }
    }
}
