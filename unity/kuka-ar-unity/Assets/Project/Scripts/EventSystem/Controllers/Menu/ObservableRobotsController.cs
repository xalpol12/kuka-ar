using UnityEngine;

public class ObservableRobotsController : MonoBehaviour
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
