using System;
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

        private void Start()
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
                    if (observableRobotsController.HttpService.Stickers.Count <= 0) return;
                    ConnectionFailed(false);
                    InitObservableRobots();
                });
        }

        private void InitObservableRobots()
        {
            var constantPanelRef = scrollList.transform.parent.GetComponent<Image>()
                .gameObject.transform.Find("ConstantPanel").GetComponent<Image>()
                .gameObject.transform;
            var http = observableRobotsController.HttpService;
            var grid = scrollList.transform.Find("Grid").GetComponent<RectTransform>().gameObject;
            var gridItem = grid.transform.Find("GridElement").GetComponent<Image>().gameObject;
            
            if (http.Robots.Count == 0)
            {
                gridItem.SetActive(false);   
                return;
            }
            gridItem.SetActive(true);
            
            if (http.Stickers.Count <= 0)
            {
                ConnectionFailed(true);
                return;
            }
            
            gridItem.transform.Find("TemplateRobotName").GetComponent<TMP_Text>().text =
                http.Robots[0].RobotName;
            gridItem.transform.Find("TemplateRobotIp").GetComponent<TMP_Text>().text =
                http.Robots[0].IpAddress;
            gridItem.transform.Find("TemplateImg").GetComponent<Image>().sprite = GetSticker(http.Robots[0].IpAddress);

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
                    newGridItem.transform.Find("TemplateImg").GetComponent<Image>().sprite =
                        GetSticker(http.Robots[i].IpAddress);
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
            // todo fix coroutine wait time to display proper status
            var ipAddress = observableRobotsController.HttpService.Robots[index].IpAddress;
            var statusText = panelRef.Find("ConnectionStatus").GetComponent<TMP_Text>();

            StartCoroutine(observableRobotsController.HttpService.PingChosenRobot(ipAddress));

            statusText.color = observableRobotsController.HttpService.RobotConnectionStatus switch
            {
                ConnectionStatus.Connected => new Color(0.176f, 0.78f, 0.439f),
                ConnectionStatus.Connecting => new Color(0.94f, 0.694f, 0.188f),
                ConnectionStatus.Disconnected => new Color(0.949f, 0.247f, 0.259f),
                _ => statusText.color
            };

            if (index > observableRobotsController.HttpService.Robots.Count - 1)
            {
                index = observableRobotsController.HttpService.Robots.Count;
            }

            panelRef.Find("CurrentIpAddress").GetComponent<TMP_Text>().text = ipAddress;
            panelRef.Find("CurrentRobotName").GetComponent<TMP_Text>().text = 
                observableRobotsController.HttpService.Robots[index].RobotName;
            panelRef.Find("CoordSystemPicker").GetComponent<Image>().sprite = GetSticker(ipAddress);
            statusText.text = observableRobotsController.HttpService.RobotConnectionStatus.ToString();
            observableRobotsController.StylingService.IsAfterItemSelect = true;
            observableRobotsController.StylingService.SliderState = LogicStates.Hiding;
        }

        private void ConnectionFailed(bool state)
        {
            scrollList.transform.Find("Grid").GetComponent<RectTransform>().gameObject
                .transform.Find("GridElement").GetComponent<Image>().gameObject.SetActive(!state);
            scrollList.transform.parent.Find("ServerError").GetComponent<Image>().gameObject.SetActive(state);
        }

        private Sprite GetSticker(string ip)
        {
            try
            {
                 return observableRobotsController.HttpService.Stickers[ip];
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return fileNotFound;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}
