using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class IpSelectController : MonoBehaviour
{
    public int id;
    public GameObject ipSelector;
    internal AddRobotRequest Request;
    internal bool ShowOptions;
    internal int TransformFactor;
    void Start()
    {
        ShowOptions = false;
        TransformFactor = 3000;
        Request = new AddRobotRequest
        {
            IpAddress = "",
            RobotName = "",
            RobotCategory = "",
        };
        
        MenuEvents.Event.OnClickIpAddress += OnClickSelectIpAddress;
    }
    
    private void OnClickSelectIpAddress(int uid)
    {
        if (id == uid)
        {
            ShowOptions = !ShowOptions;
        }
    }
}
