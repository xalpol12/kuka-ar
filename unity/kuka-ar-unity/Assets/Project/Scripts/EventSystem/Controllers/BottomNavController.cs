using UnityEngine;

public class BottomNavController : MonoBehaviour
{
    public int id;
    public GameObject bottomNavPanel;
    internal bool IsSliderHold;
    internal bool IsDocked;
    internal bool IsAfterItemSelect;
    internal int TransformFactor;

    void Start()
    {
        TransformFactor = 5000;
        IsSliderHold = false;
        IsAfterItemSelect = false;
        
        MenuEvents.Event.OnPressConstantSelectorSlider += BottomNavOnMove;
        MenuEvents.Event.OnReleaseConstantSelectorSlider += BottomNavToDockPosition;
    }
    private void BottomNavOnMove(int uid)
    {
        if (uid != id) return;
        IsSliderHold = true;
    }
    private void BottomNavToDockPosition(int uid)
    {
        if (uid != id) return;
        IsSliderHold = false;
    }
}
