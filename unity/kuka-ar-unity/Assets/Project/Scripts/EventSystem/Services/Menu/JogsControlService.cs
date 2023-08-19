using System;
using UnityEngine;

public class JogsControlService : MonoBehaviour
{
    public static JogsControlService Instance;
    
    internal bool IsBottomNavDocked;
    internal bool IsAddRobotDialogOpen;
    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        IsBottomNavDocked = true;
        IsAddRobotDialogOpen = false;
    }
}
