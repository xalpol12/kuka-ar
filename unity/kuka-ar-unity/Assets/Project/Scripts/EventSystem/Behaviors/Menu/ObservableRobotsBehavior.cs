using System.Collections.Generic;
using Project.Scripts.EventSystem.Controllers.Menu;
using Project.Scripts.EventSystem.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.EventSystem.Behaviors.Menu
{
    public class ObservableRobotsBehavior : MonoBehaviour
    {
        private ObservableRobotsController observableRobotsController;
        private GameObject scrollList;
        private Sprite fileNotFound;
        private List<GameObject> allGridItems;
        void Start()
        {
            observableRobotsController = GetComponent<ObservableRobotsController>();
            fileNotFound = Resources.Load<Sprite>("Icons/FileNotFound");

            scrollList = observableRobotsController.parentGrid;
            allGridItems = new List<GameObject>();

            InitObservableRobots();

            scrollList.transform.parent.Find("ServerError").GetComponent<Image>().transform.Find("TryAgain")
                .GetComponent<Button>().onClick.AddListener(() =>
                {
                    observableRobotsController.HttpService.OnClickDataReload(4);
                    if (observableRobotsController.HttpService.Robots.Count > 0)
                    {
                        ConnectionFailed(false);
                        InitObservableRobots();
                    }
                });
        }

        private void InitObservableRobots()
        {
            var constantPanelRef = scrollList.transform.parent.GetComponent<Image>()
                .gameObject.transform.Find("ConstantPanel").GetComponent<Image>()
                .gameObject.transform;
            var http = observableRobotsController.HttpService;

            if (http.Robots.Count == 0)
            {
                ConnectionFailed(true);
                return;
            }

            if (http.Stickers.Count == 0)
            {
                foreach (var i in http.Robots)
                {
                    http.Stickers.Add(fileNotFound);
                }
            }

            var grid = scrollList.transform.Find("Grid").GetComponent<RectTransform>().gameObject;
            var gridItem = grid.transform.Find("GridElement").GetComponent<Image>().gameObject;
            gridItem.transform.Find("TemplateRobotName").GetComponent<TMP_Text>().text =
                http.Robots[0].RobotName;
            gridItem.transform.Find("TemplateRobotIp").GetComponent<TMP_Text>().text =
                http.Robots[0].IpAddress;
            gridItem.transform.Find("TemplateImg").GetComponent<Image>().sprite = http.Stickers[0];

            gridItem.transform.GetComponent<Button>().onClick.AddListener(() =>
            {
                observableRobotsController.StylingService.MarkAsUnselected(allGridItems);
                OnSelectActions(constantPanelRef, gridItem.transform.GetSiblingIndex());
                gridItem.transform.GetComponent<Image>().sprite = observableRobotsController.StylingService.SelectedSprite;
            });
            allGridItems.Add(gridItem);

            for (var i = 1; i < observableRobotsController.HttpService.Robots.Count + 2; i++)
            {
                var newGridItem = Instantiate(gridItem, grid.transform, false);

                if (i > observableRobotsController.HttpService.Robots.Count - 1)
                {
                    newGridItem.transform.GetComponent<Image>().color = Color.clear;
                    newGridItem.transform.Find("TemplateImg").GetComponent<Image>().color = Color.clear;
                    newGridItem.transform.Find("TemplateRobotName").GetComponent<TMP_Text>().text = "";
                    newGridItem.transform.Find("TemplateRobotIp").GetComponent<TMP_Text>().text = "";
                }
                else
                {
                    newGridItem.transform.Find("TemplateRobotName").GetComponent<TMP_Text>().text =
                                    http.Robots[i].RobotName;
                    newGridItem.transform.Find("TemplateRobotIp").GetComponent<TMP_Text>().text =
                                    http.Robots[i].IpAddress;
                    gridItem.transform.Find("TemplateImg").GetComponent<Image>().sprite = http.Stickers[i - 1];
                }

                newGridItem.transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    observableRobotsController.StylingService.MarkAsUnselected(allGridItems);
                    if (gridItem.transform.GetSiblingIndex() >
                        observableRobotsController.HttpService.Robots.Count + 1)
                    {
                        return;
                    }
                    OnSelectActions(constantPanelRef, newGridItem.transform.GetSiblingIndex());
                    newGridItem.transform.GetComponent<Image>().sprite = observableRobotsController.StylingService.SelectedSprite;
                });
                allGridItems.Add(newGridItem);
            }
        }

        private void OnSelectActions(Transform panelRef, int index)
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

            if (index > observableRobotsController.HttpService.Robots.Count - 1)
            {
                index = observableRobotsController.HttpService.Robots.Count;
            }
            panelRef.Find("CurrentIpAddress").GetComponent<TMP_Text>().text =
                observableRobotsController.HttpService.Robots[index].IpAddress;
            panelRef.Find("CurrentRobotName").GetComponent<TMP_Text>().text =
                observableRobotsController.HttpService.Robots[index].RobotName;
            statusText.text = connection.ToString();
            observableRobotsController.BottomNavController.IsAfterItemSelect = true;
        }

        private void ConnectionFailed(bool state)
        {
            scrollList.transform.Find("Grid").GetComponent<RectTransform>().gameObject
                .transform.Find("GridElement").GetComponent<Image>().gameObject.SetActive(!state);
            scrollList.transform.parent.Find("ServerError").GetComponent<Image>().gameObject.SetActive(state);
        }
    }
}
