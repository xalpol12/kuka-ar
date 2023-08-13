using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogsControlService : MonoBehaviour
{
    internal bool IsBottomNavDocked;
    internal bool IsAddRobotDialogOpen;
    void Start()
    {
        IsBottomNavDocked = true;
        IsAddRobotDialogOpen = false;
    }
}
