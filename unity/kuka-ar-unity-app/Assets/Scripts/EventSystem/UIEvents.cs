using System;
using System.Collections;
using System.Collections.Generic;
using PlasticGui.WorkspaceWindow.Topbar;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public static UIEvents Current;

    private void Awake()
    {
        Current = this;
    }
    
    //rotating
    public event Action<int> OnPressStartRotation;
    public event Action<int> OnReleaseStopRotation;
    
    //scaling
    public event Action<int> OnPressStartScalingUp;
    public event Action<int> OnReleaseStopScalingUp;

    public event Action<int> OnPressStartScalingDown;
    public event Action<int> OnReleaseStopScalingDown;

    //moving
    public event Action<int> OnPressMoveLeft;
    public event Action<int> OnPressMoveRight;
    public event Action<int> OnReleaseStopMovingHorizontally;
    
    public event Action<int> OnPressMoveUp;
    public event Action<int> OnPressMoveDown;
    public event Action<int> OnReleaseStopMovingVertically;


    public void StartRotation(int id)
    {
        OnPressStartRotation?.Invoke(id);
    }

    public void StopRotation(int id)
    {
        OnReleaseStopRotation?.Invoke(id);
    }

    public void StartScalingUp(int id)
    {
        OnPressStartScalingUp?.Invoke(id);
    }

    public void StopScalingUp(int id)
    {
        OnReleaseStopScalingUp?.Invoke(id);
    }

    public void StartScalingDown(int id)
    {
        OnPressStartScalingDown?.Invoke(id);
    }

    public void StopScalingDown(int id)
    {
        OnReleaseStopScalingDown?.Invoke(id);
    }

    public void StartMovingLeft(int id)
    {
        OnPressMoveLeft?.Invoke(id);
    }

    public void StartMovingRight(int id)
    {
        OnPressMoveRight?.Invoke(id);
    }

    public void StopMovingHorizontally(int id)
    {
        OnReleaseStopMovingHorizontally?.Invoke(id);
    }

    public void StartMovingUp(int id)
    {
        OnPressMoveUp?.Invoke(id);
    }

    public void StartMovingDown(int id)
    {
        OnPressMoveDown?.Invoke(id);
    }

    public void StopMovingVertically(int id)
    {
        OnReleaseStopMovingVertically?.Invoke(id);
    }
}
