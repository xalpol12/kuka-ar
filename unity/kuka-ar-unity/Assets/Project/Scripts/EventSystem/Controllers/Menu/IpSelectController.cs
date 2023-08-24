using Project.Scripts.Connectivity.Enums;
using Project.Scripts.EventSystem.Enums;
using UnityEngine;

public class IpSelectController : MonoBehaviour
{
    public int id;
    public GameObject ipSelector;
    internal SelectableStylingService StylingService;
    internal HttpService HttpService;
    internal ButtonType ElementClicked;
    internal ButtonType PrevElementClicked;
    internal AddNewRobotService AddNewRobotService;
    internal PositioningService PositioningService;
    internal LogicStates ShowOptionsController;
    internal int TransformFactor;

    private const int GroupOffset = 1000;
    private bool showOptions;

    private void Start()
    {
        HttpService = HttpService.Instance;
        StylingService = SelectableStylingService.Instance;
        AddNewRobotService = AddNewRobotService.Instance;
        PositioningService = PositioningService.Instance;
        
        showOptions = false;
        ShowOptionsController = LogicStates.Waiting;
        TransformFactor = 7500;
        
        MenuEvents.Event.OnClickIpAddress += OnClickSelectIpAddress;
    }
    
    private void OnClickSelectIpAddress(int uid)
    {
        if (!showOptions)
        {
            PrevElementClicked = ElementClicked;
            switch (uid % GroupOffset)
            {
                case 0:
                    ElementClicked = ButtonType.IpAddress;
                    break;
                case 1:
                    ElementClicked = ButtonType.Category;
                    break;
                case 2:
                    ElementClicked = ButtonType.RobotName;
                    break;
            }
        }

        uid /= GroupOffset;
        if (id != uid) return;
        showOptions = !showOptions;
        ShowOptionsController = showOptions ? LogicStates.Running : LogicStates.Hiding;
    }
}
