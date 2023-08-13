using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class IpSelectController : MonoBehaviour
{
    public int id;
    public GameObject ipSelector;
    internal SelectableStylingService StylingService;
    internal HttpService HttpService;
    internal bool ShowOptions;
    internal int TransformFactor;
    void Start()
    {
        HttpService = HttpService.Instance;
        StylingService = SelectableStylingService.Instance;
        
        ShowOptions = false;
        TransformFactor = 3000;
        
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
