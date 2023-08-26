using Project.Scripts.EventSystem.Services.Menu;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public GameObject parentGrid;
    internal HttpService HttpService;
    internal SelectableStylingService StylingService;

    private void Start()
    {
        HttpService = HttpService.Instance;
        StylingService = SelectableStylingService.Instance;
        
            HttpService.OnClickDataReload(4);
        }
    }
}
