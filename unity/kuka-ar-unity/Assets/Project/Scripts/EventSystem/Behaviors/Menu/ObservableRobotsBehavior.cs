using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private GameObject grid;
        private GameObject gridItem;
        
        private Sprite fileNotFound;
        private Transform constantPanel;
        private Image serverError;
        private Image sticker;
        private RectTransform stickerRect;
        private Button refresh;
        private TMP_Text ipText;
        private TMP_Text nameText;
        private TMP_Text statusText;
        private TMP_Text statusHintText;
        private TMP_Text appName;

        private List<GameObject> allGridItems;
        private int checkSum;
        private int lastSelected;
        private bool performExecution;

        private void Start()
        {
            observableRobotsController = GetComponent<ObservableRobotsController>();
            fileNotFound = Resources.Load<Sprite>("Icons/FileNotFound");
            
            scrollList = observableRobotsController.parentGrid;
            var parent = scrollList.transform.parent;

            grid = scrollList.transform.Find("Grid").GetComponent<RectTransform>().gameObject;
            gridItem = grid.transform.Find("GridElement").GetComponent<Image>().gameObject;
            
            constantPanel = parent.GetComponent<Image>().gameObject
                .transform.Find("ConstantPanel").GetComponent<Image>().gameObject.transform;
            serverError = parent.Find("ServerError").GetComponent<Image>();
            sticker = constantPanel.Find("CoordSystemPicker").GetComponent<Image>().gameObject
                .transform.Find("SelectedSticker").GetComponent<Image>();
            stickerRect = sticker.GetComponent<RectTransform>();
            refresh = scrollList.transform.Find("Refresh").GetComponent<Button>();
            ipText = constantPanel.Find("CurrentIpAddress").GetComponent<TMP_Text>();
            nameText = constantPanel.Find("CurrentRobotName").GetComponent<TMP_Text>();
            statusText = constantPanel.Find("ConnectionStatus").GetComponent<TMP_Text>();
            statusHintText = constantPanel.Find("ConnectionStatusHint").GetComponent<TMP_Text>();
            appName = constantPanel.Find("AppName").GetComponent<TMP_Text>();
            
            allGridItems = new List<GameObject>();
            checkSum = -1;
            performExecution = false;
            
            StartCoroutine(InitObservableRobots());
            
            serverError.transform.Find("TryAgain")
                .GetComponent<Button>().onClick.AddListener(() =>
                {
                    StartCoroutine(DisplayLoadingSpinner(() =>
                    {
                        performExecution = true;
                        StartCoroutine(ServerInvoker.Invoker.GetRobots());
                        StartCoroutine(ServerInvoker.Invoker.GetStickers(() =>
                        {
                            StartCoroutine(DestroyListEntries());
                        }));
                    }));
                });
            
            refresh.onClick.AddListener(() =>
            {
                StartCoroutine(ServerInvoker.Invoker.GetRobots(() =>
                {
                    if (!observableRobotsController.WebDataStorage.Robots
                            .Any(robot => robot.IpAddress == observableRobotsController.LogicService.SelectedIpAddress
                                          && robot.Name == observableRobotsController.LogicService.SelectedName))
                    {
                        observableRobotsController.StylingService.MarkAsUnselected(allGridItems, true);
                        sticker.sprite = observableRobotsController.StylingService.DefaultSticker;
                        AdjustImageMargin(true);
                        ConstantPanelInfoText(false);
                    }
                    StartCoroutine(DestroyListEntries());
                }));
            });
        }

        private void Update()
        {
            if (!observableRobotsController.WebDataStorage.IsAfterRobotSave) return;
            StartCoroutine(DestroyListEntries());
            ConnectionStatusCheckHandler();
            observableRobotsController.WebDataStorage.IsAfterRobotSave = false;
        }

        private IEnumerator InitObservableRobots()
        {
            if (scrollList is null) yield break;
            var storage = observableRobotsController.WebDataStorage;
            
            if (storage.Stickers.Count <= 0)
            {
                refresh.gameObject.SetActive(false);
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
                StartCoroutine(OnSelectActions(gridItem.transform.GetSiblingIndex()));
                gridItem.transform.GetComponent<Image>().sprite = 
                    observableRobotsController.StylingService.SelectedSprite;
            });
            gridItem.SetActive(true);
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
                    if (newGridItem.transform.GetSiblingIndex() >
                        observableRobotsController.WebDataStorage.Robots.Count - 1)
                    {
                        return;
                    }
                    StartCoroutine(OnSelectActions(newGridItem.transform.GetSiblingIndex()));
                    newGridItem.transform.GetComponent<Image>().sprite =
                        observableRobotsController.StylingService.SelectedSprite;
                });
                allGridItems.Add(newGridItem);
            }
            if (storage.Robots.Count + 2 > allGridItems.Count)
            {
                for (var i = storage.Robots.Count + 2; i < allGridItems.Count; i++)
                {
                    Destroy(allGridItems[i]);
                    allGridItems.RemoveAt(i);
                }
            }
            
            yield return null;
        }

        private IEnumerator OnSelectActions(int index)
        {
            var ipAddress = observableRobotsController.WebDataStorage.Robots[index].IpAddress;
            ConstantPanelInfoText(true);

            if (index > observableRobotsController.WebDataStorage.Robots.Count - 1)
            {
                index = observableRobotsController.WebDataStorage.Robots.Count;
            }

            ipText.text = ipAddress;
            nameText.text = observableRobotsController.WebDataStorage.Robots[index].Name;
            sticker.sprite = GetSticker(ipAddress);
            AdjustImageMargin(false);
            observableRobotsController.RobotsHandler.ChangeSelectedRobotIP(ipAddress);

            observableRobotsController.LogicService.IsAfterItemSelect = true;
            observableRobotsController.LogicService.SelectedIpAddress = ipAddress;
            observableRobotsController.LogicService.SelectedName = nameText.text;
            observableRobotsController.LogicService.SliderState = LogicStates.Hiding;
            lastSelected = index;
            yield return null;
        }

        private IEnumerator DestroyListEntries()
        {
            allGridItems = new List<GameObject>();
            foreach (var child in grid.GetComponentsInChildren<RectTransform>().ToList()
                         .Where(child => child.name.Contains("(Clone)")))
            {
                Destroy(child.gameObject);
            }
            
            StartCoroutine(InitObservableRobots());
            yield return null;
        }

        private void ConstantPanelInfoText(bool isRobotChosen)
        {
            nameText.gameObject.SetActive(isRobotChosen);
            ipText.gameObject.SetActive(isRobotChosen);
            statusText.gameObject.SetActive(isRobotChosen);
            statusHintText.gameObject.SetActive(isRobotChosen);
            appName.gameObject.SetActive(!isRobotChosen);
        }

        private void AdjustImageMargin(bool inFrame)
        {
            var val = inFrame ? new[] { -15, 15, 35, 15 } : new[] { -10, -15, -10, -15 }; 
            stickerRect.offsetMax = new Vector2(val[1],val[0]) * -1;
            stickerRect.offsetMin = new Vector2(val[3],val[2]);
            stickerRect.localScale = Vector3.one * (1 + Math.Abs(val[0] / 100));
        }
        
        private void ConnectionStatusCheckHandler()
        {
            statusText.text = observableRobotsController.WebDataStorage.RobotConnectionStatus.ToString();
            statusText.color = observableRobotsController.WebDataStorage.RobotConnectionStatus switch
            {
                ConnectionStatus.Connected => new Color(0.176f, 0.78f, 0.439f),
                ConnectionStatus.Connecting => new Color(0.94f, 0.694f, 0.188f),
                ConnectionStatus.Disconnected => new Color(0.949f, 0.247f, 0.259f),
                _ => statusText.color
            };
        }

        private void ConnectionFailed(bool state)
        {
            performExecution = false;
            gridItem.SetActive(!state);
            refresh.gameObject.SetActive(!state);
            serverError.gameObject.SetActive(state);
        }

        private Sprite GetSticker(string ip)
        {
            try
            {
                 return observableRobotsController.WebDataStorage.Stickers[ip];
            }
            catch (KeyNotFoundException e)
            {
                Debug.unityLogger.Log(e.Message);
                return fileNotFound;
            }
            catch (Exception e)
            {
                Debug.unityLogger.Log(e.Message);
            }

            return null;
        }

        private IEnumerator DisplayLoadingSpinner(Action action)
        {
            var time = 0f;
            var webStore = observableRobotsController.WebDataStorage;
            
            serverError.gameObject.SetActive(false);
            observableRobotsController.Spinner.SetActive(true);
            action();
            
            while (webStore.LoadingSpinner.Any(spinner => spinner.Value))
            {
                if (time > webStore.animationTimeout || !performExecution) break;
                time += Time.deltaTime;

                yield return null;
            }
            observableRobotsController.Spinner.SetActive(false);
            serverError.gameObject.SetActive(true);
            
            if (webStore.Stickers.Count == 0) yield break;
            ConnectionFailed(false);
        }
    }
}
