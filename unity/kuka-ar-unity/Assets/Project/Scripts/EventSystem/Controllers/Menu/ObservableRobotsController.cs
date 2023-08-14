using UnityEngine;

public class ObservableRobotsController : MonoBehaviour
{
    public int id;
    public GameObject parentGrid;
    internal HttpService HttpService;
    internal BottomNavController BottomNavController;
    internal SelectableStylingService StylingService;
    void Start()
    {
        HttpService = HttpService.Instance;
        StylingService = SelectableStylingService.Instance;
        BottomNavController = BottomNavController.Instance;

        MenuEvents.Event.OnSelectFromList += SelectClickedRobot;
    }
    // TODO REFACTOR THIS SECTION IN FREE TIME TO MAKE USE OF MVC STRATEGY
    private void SelectClickedRobot(int uid)
    {
        // if (id == uid)
        // {
        //     Debug.Log("child index: " + parentGrid.transform.GetSiblingIndex());
        // }
    }
}
