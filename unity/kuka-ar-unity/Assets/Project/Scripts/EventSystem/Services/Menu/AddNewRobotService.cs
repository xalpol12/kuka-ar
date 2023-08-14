using UnityEngine;

public class AddNewRobotService : MonoBehaviour
{
    public static AddNewRobotService Instance;

    internal bool ResetSelectState;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ResetSelectState = false;
    }
}
