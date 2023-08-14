using UnityEngine;

public class BottomNavController : MonoBehaviour
{
    public static BottomNavController Instance;
    
    public int id;
    public GameObject bottomNavPanel;
    internal SelectableStylingService stylingService;
    internal bool IsSliderHold;
    internal bool IsAfterItemSelect;
    internal bool IsCirclePressed;
    internal int TransformFactor;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        stylingService = SelectableStylingService.Instance;
        
        TransformFactor = 5000;
        IsSliderHold = false;
        IsCirclePressed = false;
        IsAfterItemSelect = false;
        
        MenuEvents.Event.OnPressConstantSelectorSlider += BottomNavOnMove;
        MenuEvents.Event.OnReleaseConstantSelectorSlider += BottomNavToDockPosition;
        MenuEvents.Event.OnPointerPressCircle += CirclePress;
        MenuEvents.Event.OnPointerPressedCircle += CirclePressed;
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

    private void CirclePress(int uid)
    {
        if (id == uid)
        {
            IsCirclePressed = true;
        }
    }

    private void CirclePressed(int uid)
    {
        if (id == uid)
        {
            IsCirclePressed = false;
        }
    }
}
