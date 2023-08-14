using Project.Scripts.Connectivity.Enums;
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
    internal bool ShowOptions;
    internal int TransformFactor;
    
    private const int GroupOffset = 1000;
    void Start()
    {
        HttpService = HttpService.Instance;
        StylingService = SelectableStylingService.Instance;
        AddNewRobotService = AddNewRobotService.Instance;
        
        ShowOptions = false;
        TransformFactor = 7500;
        
        MenuEvents.Event.OnClickIpAddress += OnClickSelectIpAddress;
    }
    
    private void OnClickSelectIpAddress(int uid)
    {
        if (!ShowOptions)
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
        if (id == uid)
        {
            ShowOptions = !ShowOptions;
        }
    }
}
