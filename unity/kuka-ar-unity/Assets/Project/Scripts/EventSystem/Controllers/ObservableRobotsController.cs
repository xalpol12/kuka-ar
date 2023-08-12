using UnityEngine;

public class ObservableRobotsController : MonoBehaviour
{
    public int id;
    public GameObject parentGrid;
    void Start()
    {
        MenuEvents.Event.OnSelectFromList += SelectClickedRobot;
    }

    private void SelectClickedRobot(int uid)
    {
        // if (id == uid)
        // {
        //     Debug.Log("child index: " + parentGrid.transform.GetSiblingIndex());
        // }
    }
}
