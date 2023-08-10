using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IpSelectController : MonoBehaviour
{
    public int id;
    public GameObject ipSelector;
    internal bool ShowOptions;
    internal int TransformFactor;
    void Start()
    {
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
