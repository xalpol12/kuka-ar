using UnityEngine;

public class BottomNavController : MonoBehaviour
{
    public int id;
    public GameObject bottomNavPanel;
    public GameObject overlayPanel;
    public GameObject constantPanel;
    internal bool IsSliderHold;
    internal int transformFactor;

    void Start()
    {
        transformFactor = 5000;
        IsSliderHold = false;
        
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
