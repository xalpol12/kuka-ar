using System.Collections.Generic;
using Project.Scripts.EventSystem.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObservableRobotsBehavior : MonoBehaviour
{
    private ObservableRobotsController observableRobotsController;
    private BottomNavController bottomNavController;
    private SelectableStylingService stylingService;
    private GameObject scrollList;
    private List<GameObject> allGridItems;
    void Start()
    {
        observableRobotsController = GetComponent<ObservableRobotsController>();
        bottomNavController = FindObjectOfType<BottomNavController>();
        stylingService = FindObjectOfType<SelectableStylingService>();
        scrollList = observableRobotsController.parentGrid;
        allGridItems = new List<GameObject>();

        var constantPanelRef = scrollList.transform.parent.GetComponent<Image>()
            .gameObject.transform.Find("ConstantPanel").GetComponent<Image>()
            .gameObject.transform;
        var grid = scrollList.transform.Find("Grid").GetComponent<RectTransform>().gameObject;
        var gridItem = grid.transform.Find("GridElement").GetComponent<Image>().gameObject;
        gridItem.transform.GetComponent<Button>().onClick.AddListener(() =>
        {
            stylingService.MarkAsUnselected(allGridItems);
            OnSelectActions(constantPanelRef, "192.168.100.111", "Random robot name");
            gridItem.transform.GetComponent<Image>().sprite = stylingService.selectedSprite;
        });
        allGridItems.Add(gridItem);
        
        for (var i = 0; i < 25; i++)
        {
            var newGridItem = Instantiate(gridItem, grid.transform, false);
            var ipAddressText = newGridItem.transform.Find("TemplateRobotName")
                .GetComponent<TMP_Text>().text = "Robot 000" + i;
            var robotName = newGridItem.transform.Find("TemplateRobotIp")
                .GetComponent<TMP_Text>().text = "192.168.100." + i;
            
            newGridItem.transform.GetComponent<Button>().onClick.AddListener(() =>
            {
                stylingService.MarkAsUnselected(allGridItems);
                OnSelectActions(constantPanelRef, ipAddressText, robotName);
                newGridItem.transform.GetComponent<Image>().sprite = stylingService.selectedSprite;
            });
            allGridItems.Add(newGridItem);
        }
    }

    private void OnSelectActions(Transform panelRef, string ipAddressText, string robotName)
    {
        var connection = ConnectionStatus.Connecting;
        var statusText = panelRef.Find("ConnectionStatus").GetComponent<TMP_Text>();
        switch (connection)
        {
            case ConnectionStatus.Connected:
                statusText.color = new Color(0.176f, 0.78f, 0.439f);
                break;
            case ConnectionStatus.Connecting:
                statusText.color = new Color(0.94f, 0.694f,0.188f);
                break;
            case ConnectionStatus.Disconnected:
                statusText.color = new Color(0.949f, 0.247f, 0.259f);
                break;
        }
        
        panelRef.Find("CurrentIpAddress").GetComponent<TMP_Text>().text = ipAddressText;
        panelRef.Find("CurrentRobotName").GetComponent<TMP_Text>().text = robotName;
        statusText.text = connection.ToString();
        bottomNavController.IsAfterItemSelect = true;
    }
}
