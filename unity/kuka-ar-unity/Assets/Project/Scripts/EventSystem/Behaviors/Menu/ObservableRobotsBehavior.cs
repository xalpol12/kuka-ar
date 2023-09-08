using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Connectivity.Http.Requests;
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
        private GameObject grid;
        private GameObject gridItem;
        private int lastSelected;

        private void Start()
        {
            observableRobotsController = GetComponent<ObservableRobotsController>();
            fileNotFound = Resources.Load<Sprite>("Icons/FileNotFound");
            
            scrollList = observableRobotsController.parentGrid;
            allGridItems = new List<GameObject>();
            grid = scrollList.transform.Find("Grid").GetComponent<RectTransform>().gameObject;
            gridItem = grid.transform.Find("GridElement").GetComponent<Image>().gameObject;
            
            StartCoroutine(ServerInvoker.Invoker.GetRobots());
            
            StartCoroutine(InitObservableRobots());
        
            scrollList.transform.parent.Find("ServerError").GetComponent<Image>().transform.Find("TryAgain")
                .GetComponent<Button>().onClick.AddListener(() =>
                {
                    StartCoroutine(ServerInvoker.Invoker.GetRobots());
                    StartCoroutine(ServerInvoker.Invoker.GetStickers());
                    if (observableRobotsController.WebDataStorage.Stickers.Count <= 0) return;
                    ConnectionFailed(false);
                    StartCoroutine(InitObservableRobots());
                });
        }

        private void Update()
        {
            if (observableRobotsController.WebDataStorage.IsAfterRobotSave)
            { 
                StartCoroutine(DestroyListEntries()); 
                observableRobotsController.WebDataStorage.IsAfterRobotSave = false;
            }
        }

        private IEnumerator InitObservableRobots()
        {
            if (scrollList is null) yield break;
            var constantPanelRef = scrollList.transform.parent.GetComponent<Image>()
                .gameObject.transform.Find("ConstantPanel").GetComponent<Image>()
                .gameObject.transform;
            var storage = observableRobotsController.WebDataStorage;
            
            
            if (storage.Stickers.Count <= 0)
            {
                ConnectionFailed(true);
                yield break;
            }

            if (storage.Robots.Count < 1)
            {
                gridItem.SetActive(false);
                yield break;
            }
            
            gridItem.transform.Find("TemplateRobotName").GetComponent<TMP_Text>().text =
                storage.Robots[0].Name;
            gridItem.transform.Find("TemplateRobotIp").GetComponent<TMP_Text>().text =
                storage.Robots[0].IpAddress;
            gridItem.transform.Find("TemplateImg").GetComponent<Image>().sprite = GetSticker(storage.Robots[0].IpAddress);

            gridItem.transform.GetComponent<Button>().onClick.AddListener(() =>
            {
                observableRobotsController.StylingService.MarkAsUnselected(allGridItems, true);
                StartCoroutine(OnSelectActions(constantPanelRef, gridItem.transform.GetSiblingIndex()));
                gridItem.transform.GetComponent<Image>().sprite = 
                    observableRobotsController.StylingService.SelectedSprite;
            });
            allGridItems.Add(gridItem);

            for (var i = 1; i < observableRobotsController.WebDataStorage.Robots.Count + 2; i++)
            {
                var newGridItem = Instantiate(gridItem, grid.transform, false);
                
                if (i > observableRobotsController.WebDataStorage.Robots.Count - 1)
                {
                    newGridItem.transform.GetComponent<Image>().color = Color.clear;
                    newGridItem.transform.Find("TemplateImg").GetComponent<Image>().color = Color.clear;
                    newGridItem.transform.Find("TemplateRobotName").GetComponent<TMP_Text>().text = "";
                    newGridItem.transform.Find("TemplateRobotIp").GetComponent<TMP_Text>().text = "";
                }
                else
                {
                    newGridItem.transform.GetComponent<Image>().sprite = i == lastSelected
                            ? observableRobotsController.StylingService.SelectedSprite
                            : observableRobotsController.StylingService.DefaultNoFrame;
                    newGridItem.transform.Find("TemplateRobotName").GetComponent<TMP_Text>().text =
                        storage.Robots[i].Name;
                    newGridItem.transform.Find("TemplateRobotIp").GetComponent<TMP_Text>().text =
                        storage.Robots[i].IpAddress;
                    newGridItem.transform.Find("TemplateImg").GetComponent<Image>().sprite =
                        GetSticker(storage.Robots[i].IpAddress);
                }

                newGridItem.transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    observableRobotsController.StylingService.MarkAsUnselected(allGridItems, true);
                    if (gridItem.transform.GetSiblingIndex() >
                        observableRobotsController.WebDataStorage.Robots.Count + 1)
                    {
                        return;
                    }
                    StartCoroutine(OnSelectActions(constantPanelRef, newGridItem.transform.GetSiblingIndex()));
                    newGridItem.transform.GetComponent<Image>().sprite =
                        observableRobotsController.StylingService.SelectedSprite;
                });
                allGridItems.Add(newGridItem);
            }

            yield return null;
        }

        private IEnumerator OnSelectActions(Transform panelRef, int index)
        {
            var ipAddress = observableRobotsController.WebDataStorage.Robots[index].IpAddress;
            var statusText = panelRef.Find("ConnectionStatus").GetComponent<TMP_Text>();

            StartCoroutine(ConnectionStatusCheckHandler(statusText, ipAddress));

            if (index > observableRobotsController.WebDataStorage.Robots.Count - 1)
            {
                index = observableRobotsController.WebDataStorage.Robots.Count;
            }

            panelRef.Find("CurrentIpAddress").GetComponent<TMP_Text>().text = ipAddress;
            panelRef.Find("CurrentRobotName").GetComponent<TMP_Text>().text = 
                observableRobotsController.WebDataStorage.Robots[index].Name;
            panelRef.Find("CoordSystemPicker").GetComponent<Image>().sprite = GetSticker(ipAddress);
            observableRobotsController.LogicService.IsAfterItemSelect = true;
            observableRobotsController.LogicService.SelectedIpAddress = ipAddress;
            observableRobotsController.LogicService.SliderState = LogicStates.Hiding;
            lastSelected = index;
            yield return null;
        }

        private IEnumerator DestroyListEntries()
        {
            allGridItems = new List<GameObject>();
            foreach (var child in grid.GetComponentsInChildren<RectTransform>())
            {
                if (child.name.Contains("(Clone)"))
                {
                    Destroy(child.gameObject);
                }
            }
    
            StartCoroutine(InitObservableRobots());
            yield return null;
        }

        private IEnumerator ConnectionStatusCheckHandler(TMP_Text statusText, string ipAddress)
        {
            var isFinalRefresh = true;
            observableRobotsController.WebDataStorage.RobotConnectionStatus = ConnectionStatus.Connecting;
            StartCoroutine(ServerInvoker.Invoker.PingRobot(ipAddress));
            
            while (observableRobotsController.WebDataStorage.RobotConnectionStatus == ConnectionStatus.Connecting ||
                   isFinalRefresh)
            {
                statusText.text = observableRobotsController.WebDataStorage.RobotConnectionStatus.ToString();
                statusText.color = observableRobotsController.WebDataStorage.RobotConnectionStatus switch
                {
                    ConnectionStatus.Connected => new Color(0.176f, 0.78f, 0.439f),
                    ConnectionStatus.Connecting => new Color(0.94f, 0.694f, 0.188f),
                    ConnectionStatus.Disconnected => new Color(0.949f, 0.247f, 0.259f),
                    _ => statusText.color
                };

                isFinalRefresh = observableRobotsController.WebDataStorage.RobotConnectionStatus ==
                                 ConnectionStatus.Connecting;
                yield return null;
            }
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
                 return observableRobotsController.WebDataStorage.Stickers[ip];
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
