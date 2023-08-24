using UnityEngine;

public class ObservableRobotsController : MonoBehaviour
{
    public GameObject parentGrid;
    internal HttpService HttpService;
    internal SelectableStylingService StylingService;
    void Start()
    {
        HttpService = HttpService.Instance;
        StylingService = SelectableStylingService.Instance;
        
        HttpService.OnClickDataReload(4);
    }
}
