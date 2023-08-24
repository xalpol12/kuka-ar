using UnityEngine;

public class ConnectionTestBehavior : MonoBehaviour
{
    private ConnectionTestController controller;
    
    private void Start()
    {
        controller = GetComponent<ConnectionTestController>();
    }
}
