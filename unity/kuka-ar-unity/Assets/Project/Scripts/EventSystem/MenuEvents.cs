using System;
using UnityEngine;
using Action = Unity.Plastic.Antlr3.Runtime.Misc.Action;

public class MenuEvents : MonoBehaviour
{
    public static MenuEvents Event;
    private void Awake()
    {
        Event = this;
    }

    public event Action<int> OnPressConstantSelectorSlider;
    public event Action<int> OnReleaseConstantSelectorSlider;
    public event Action<int> OnClickJog;
    public event Action<int> OnClickAddNewRobot;
    public event Action<int> OnRobotSave;
    public event Action<int> OnDragAddNewRobot;
    public event Action<int> OnDropAddNewRobot;
    public event Action<int> OnClickIpAddress;
    public event Action<int> OnSelectFromList; 

    public void BottomNavOnMove(int id)
    {
        OnPressConstantSelectorSlider?.Invoke(id);
    }

    public void BottomNavToDockPosition(int id)
    {
        OnReleaseConstantSelectorSlider?.Invoke(id);
    }

    public void ShowJogs(int id)
    {
        OnClickJog?.Invoke(id);
    }

    public void ShowAddRobotDialog(int id)
    {
        OnClickAddNewRobot?.Invoke(id);
    }

    public void OnClickRobotSave(int id)
    {
        OnRobotSave?.Invoke(id);
    }

    public void OnDragRobotDialog(int id)
    {
        OnDragAddNewRobot?.Invoke(id);
    }

    public void OnDropRobotDialog(int obj)
    {
        OnDropAddNewRobot?.Invoke(obj);
    }

    public void OnClickIpAddressSelect(int id)
    {
        OnClickIpAddress?.Invoke(id);
    }

    public void OnSelectListElement(int id)
    {
        OnSelectFromList?.Invoke(id);
    }
}
