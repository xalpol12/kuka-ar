using UnityEngine;

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
    }
}
