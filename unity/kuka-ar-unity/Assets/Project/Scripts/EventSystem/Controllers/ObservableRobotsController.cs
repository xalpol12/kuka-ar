using Project.Scripts.Connectivity.Models.AggregationClasses;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class ObservableRobotsController : MonoBehaviour
{
    public int id;
    public GameObject parentGrid;
    internal AddRobotRequest Request;
    void Start()
    {
        Request = new AddRobotRequest
        {
            IpAddress = "",
            RobotName = "",
            RobotCategory = "",
        };
        
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
