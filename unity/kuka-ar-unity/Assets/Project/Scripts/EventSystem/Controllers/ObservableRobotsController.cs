using UnityEngine;

public class ObservableRobotsController : MonoBehaviour
{
    public int id;
    public GameObject parentGrid;
    void Start()
    {
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
