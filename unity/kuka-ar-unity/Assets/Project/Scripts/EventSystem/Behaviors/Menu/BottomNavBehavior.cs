using System.Collections;
using Project.Scripts.EventSystem.Enums;
using UnityEngine;
using UnityEngine.UI;

public class BottomNavBehavior : MonoBehaviour
{
    [SerializeField] private float pullMenuScreenMaxHeight = 0.34f;
    
    private BottomNavController bottomNav;
    private JogsControlService service;
    private GameObject constantPanel;
    private GameObject scrollList;
    private GameObject plusImage;
    private Image circleImage;
    private Vector3 dockPosition;
    private const int ErrorOffset = 25;
    void Start()
    {
        bottomNav = GetComponent<BottomNavController>();
        service = JogsControlService.Instance;
        
        var bottomPanel = bottomNav.transform;
        scrollList = bottomPanel.Find("ViewCoordList").GetComponent<Image>().gameObject;
        constantPanel = bottomPanel.Find("ConstantPanel").GetComponent<Image>().gameObject;
        plusImage = constantPanel.transform.Find("AddButtonContainer").GetComponent<RectTransform>().gameObject
            .transform.Find("AddButton").GetComponent<RectTransform>().gameObject;
        circleImage = constantPanel.transform.Find("AddButtonContainer").GetComponent<RectTransform>().gameObject
            .transform.Find("AddButtonCircle").GetComponent<Image>();
        
        dockPosition = bottomPanel.position;
    }

    void Update()
    {
        if (bottomNav.SliderState == LogicStates.Running)
        {
            StartCoroutine(BottomMenuPositionHandler());
        }
        else if (bottomNav.SliderState == LogicStates.Hiding)
        {
            StartCoroutine(bottomNav.StylingService.IsAfterItemSelect ?
                CloseObservableRobotsList() : AutoDestinationPull());
        }
        StartCoroutine(AddNewRobotAnimation());
        StartCoroutine(ConstantPanelVisibilityHandler());
        StartCoroutine(JogsExpandHandler());
    }

    private IEnumerator BottomMenuPositionHandler()
    {
        var menuPosition = new Vector3(dockPosition.x, Input.mousePosition.y) ;
        if (menuPosition.y > Screen.height * pullMenuScreenMaxHeight)
        {
            menuPosition.y = Screen.height * pullMenuScreenMaxHeight;
            bottomNav.SliderState = LogicStates.Waiting;
            yield break;
        }

        if (menuPosition.y < dockPosition.y)
        {
            menuPosition.y = dockPosition.y;
            yield break;
        }
        bottomNav.transform.position = menuPosition;
        yield return null;
    }

    private IEnumerator AutoDestinationPull()
    {
        if (!bottomNav.transform.position.y.Equals(dockPosition.y))
        {
            Vector3 translation;
            if (transform.position.y > (Screen.height * pullMenuScreenMaxHeight + dockPosition.y) / 2)
            {
                translation = Vector3.up * (Time.deltaTime * bottomNav.TransformFactor);
            }
            else
            {
                translation = Vector3.down * (Time.deltaTime * bottomNav.TransformFactor);
            }
            
            var newPosition = bottomNav.transform.position + translation;
            if (newPosition.y > Screen.height * pullMenuScreenMaxHeight)
            {
                bottomNav.SliderState = LogicStates.Waiting;
                yield break;
            }
            
            if (newPosition.y < dockPosition.y)
            {
                bottomNav.transform.position = dockPosition;
                bottomNav.SliderState = LogicStates.Waiting;
                yield break;
            }
                    
            bottomNav.transform.Translate(translation);
            yield return null;
        }
    }

    private IEnumerator CloseObservableRobotsList()
    {
        var translation = Vector3.down * (Time.deltaTime * bottomNav.TransformFactor);
        var newPosition = bottomNav.transform.position + translation;

        if (newPosition.y < dockPosition.y)
        {
            bottomNav.StylingService.IsAfterItemSelect = false;
            yield break;
        }
        
        bottomNav.transform.Translate(translation);
        yield return null;
    }

    private IEnumerator ConstantPanelVisibilityHandler()
    {
        if (bottomNav.transform.position.y > (Screen.height * pullMenuScreenMaxHeight + dockPosition.y) / 2)
        {
            constantPanel.SetActive(false);
            scrollList.SetActive(true);
            yield break;
        }
        
        constantPanel.SetActive(true);
        scrollList.SetActive(false);
    }

    private IEnumerator JogsExpandHandler()
    {
        service.IsBottomNavDocked = bottomNav.bottomNavPanel.transform.position.y - ErrorOffset <= dockPosition.y;
        yield return null;
    }

    private IEnumerator AddNewRobotAnimation()
    {
        plusImage.SetActive(!bottomNav.IsCirclePressed);
        circleImage.sprite = bottomNav.IsCirclePressed ?
            bottomNav.StylingService.PressedAddIcon : bottomNav.StylingService.DefaultAddIcon;
        yield return null;
    }
}
