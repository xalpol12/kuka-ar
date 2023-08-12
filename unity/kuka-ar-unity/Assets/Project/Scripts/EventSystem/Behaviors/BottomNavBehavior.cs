using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BottomNavBehavior : MonoBehaviour
{
    private BottomNavController bottomNav;
    private GameObject constantPanel;
    private GameObject scrollList;
    [SerializeField] private float pullMenuScreenMaxHeight = 0.34f;
    private Vector3 dockPosition;
    private const int ErrorOffset = 25;
    void Start()
    {
        bottomNav = GetComponent<BottomNavController>();

        var bottomPanel = bottomNav.transform;
        scrollList = bottomPanel.Find("ViewCoordList").GetComponent<Image>().gameObject;
        constantPanel = bottomPanel.Find("ConstantPanel").GetComponent<Image>().gameObject;
        
        dockPosition = bottomPanel.position;
    }

    void Update()
    {
        
        if (bottomNav.IsSliderHold)
        {
            bottomNav.transform.position = BottomMenuPositionHandler();
        }
        else
        {
            if (bottomNav.IsAfterItemSelect)
            {
                CloseObservableRobotsList();
            }
            else
            {
                AutoDestinationPull();
            }
        }
        ConstantPanelVisibilityHandler();
        JogsExpandHandler();
    }

    private Vector3 BottomMenuPositionHandler()
    {
        var menuPosition = Vector3.up ;
        menuPosition.y *= Input.mousePosition.y - (Screen.height * 0.105f - dockPosition.y);
        menuPosition.x = dockPosition.x;

        if (menuPosition.y > Screen.height * pullMenuScreenMaxHeight)
        {
            menuPosition.y = Screen.height * pullMenuScreenMaxHeight;
        }

        if (menuPosition.y < dockPosition.y)
        {
            menuPosition.y = dockPosition.y;
        }
        return menuPosition;
    }

    private void AutoDestinationPull()
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
                translation = new Vector3();
            }
            
            if (newPosition.y < dockPosition.y)
            {
                translation = new Vector3();
            }
                    
            bottomNav.transform.Translate(translation);
        }
    }

    private void CloseObservableRobotsList()
    {
        var translation = Vector3.down * (Time.deltaTime * bottomNav.TransformFactor);
        var newPosition = bottomNav.transform.position + translation;

        if (newPosition.y < dockPosition.y)
        {
            translation = new Vector3();
            bottomNav.IsAfterItemSelect = false;
        }
        
        bottomNav.transform.Translate(translation);
    }

    private void ConstantPanelVisibilityHandler()
    {
        if (bottomNav.transform.position.y > (Screen.height * pullMenuScreenMaxHeight + dockPosition.y) / 2)
        {
            constantPanel.SetActive(false);
            scrollList.SetActive(true);
        }
        else
        {
            constantPanel.SetActive(true);
            scrollList.SetActive(false);
        }
    }

    private void JogsExpandHandler()
    {
        bottomNav.IsDocked = bottomNav.transform.position.y - ErrorOffset <= dockPosition.y;
    }
}
