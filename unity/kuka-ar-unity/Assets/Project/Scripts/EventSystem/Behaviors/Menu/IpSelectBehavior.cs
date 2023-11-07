using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Connectivity.Enums;
using Project.Scripts.Connectivity.Http.Requests;
using Project.Scripts.EventSystem.Controllers.Menu;
using Project.Scripts.EventSystem.Enums;
using Project.Scripts.EventSystem.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.EventSystem.Behaviors.Menu
{
    public class IpSelectBehavior : MonoBehaviour
    {
        private IpSelectController selectController;
        private GameObject ipList;
        private GameObject grid;
        private GameObject gridItem;
        private GameObject serverError;
        
        private TMP_Text buttonText;
        private TMP_Text chosenIpText;
        private TMP_Text chosenNameText;
        private TMP_Text chosenCategoryText;

        private List<GameObject> allIpAddresses;
        private Vector3 selectIpHomePosition;
        private bool isDialogOpen;
        private void Start()
        {
            selectController = GetComponent<IpSelectController>();
            ipList = selectController.ipSelector.transform.Find("IpAddressList").GetComponent<RectTransform>()
                .gameObject;
            grid = ipList.transform.Find("IpAddressGrid").GetComponent<RectTransform>().gameObject;
            gridItem = grid.transform.Find("IpAddressGridElement").GetComponent<RectTransform>().gameObject;
            serverError = grid.transform.Find("ServerError").gameObject;

            var parent = selectController.ipSelector.transform.parent;
            buttonText = parent.transform.gameObject
                .transform.Find("Button").GetComponent<Button>().gameObject
                .transform.Find("SaveCloseButton").GetComponent<TMP_Text>();
            chosenIpText = parent.Find("IpAddress").GetComponent<RectTransform>().gameObject.transform
                .Find("Label").GetComponent<TMP_Text>();
            chosenCategoryText = parent.Find("ChosenCategory").GetComponent<RectTransform>().gameObject.transform
                .Find("CategoryLabel").GetComponent<TMP_Text>();
            chosenNameText = parent.Find("RobotName").GetComponent<RectTransform>().gameObject.transform
                .Find("NameLabel").GetComponent<TMP_Text>();

            allIpAddresses = new List<GameObject>();
            isDialogOpen = false;
        
            InitListLogic();

            selectIpHomePosition = selectController.ipSelector.transform.position;
            serverError.transform.Find("TryAgain").GetComponent<Button>().onClick.AddListener(() =>
                {
                    StartCoroutine(ServerInvoker.Invoker.GetConfiguredRobots());
                    StartCoroutine(HandleDataRefresh());
                });
        }

        private void Update()
        {
            if (selectController.ShowOptionsController == LogicStates.Running)
            {
                StartCoroutine(UpdateListData());
                StartCoroutine(ShowIpSelectDialog());
                if (selectController.AddNewRobotService.ResetSelectState)
                {
                    StartCoroutine(ResetStates());
                }

                selectController.ShowOptionsController = LogicStates.Waiting;
            }
            else if (selectController.ShowOptionsController == LogicStates.Hiding)
            {
                StartCoroutine(HideIpSelectDialog());
                selectController.ShowOptionsController = LogicStates.Waiting;
            }

            if (!Input.GetKey(KeyCode.Escape) || !isDialogOpen) return;
            selectController.ShowOptionsController = LogicStates.Hiding;
            StartCoroutine(HideIpSelectDialog());

        }

        private void InitListLogic()
        {
            if (selectController.DataStorage.availableRobotsNames.Count == 0)
            {
                ServerFailure(true);
                return;
            }
            
            foreach (var (ip, i) in selectController.DataStorage.availableIps.WithIndex())
            {
                if (ip == selectController.DataStorage.availableIps.First())
                {
                    MakeListEntry(ip, $"{ButtonType.IpAddress}-{i}", gridItem);
                    continue;
                }

                MakeListEntry(ip, $"{ButtonType.IpAddress}-{i}");
            }

            foreach (var (category, i) in selectController.DataStorage.availableCategoryNames.WithIndex())
            {
                MakeListEntry(category, $"{ButtonType.Category}-{i}");
            }

            foreach (var (robot, i) in selectController.DataStorage.availableRobotsNames.WithIndex())
            {
                MakeListEntry(robot, $"{ButtonType.RobotName}-{i}");
            }
        }

        private IEnumerator ShowIpSelectDialog()
        {
            var translation = Vector3.right * (Time.deltaTime * selectController.TransformFactor);
            var newPose = selectController.ipSelector.transform.position + translation;
            while (newPose.x < selectController.PositioningService.bestFitPosition.x)
            {
                selectController.ipSelector.transform.Translate(translation);
                translation = Vector3.right * (Time.deltaTime * selectController.TransformFactor);
                newPose = selectController.ipSelector.transform.position + translation;
                
                yield return null;
            }
            isDialogOpen = true;
            selectController.AddNewRobotService.IsSelectDialogOpen = isDialogOpen;
            selectController.ipSelector.transform.position = 
                new Vector3(selectController.PositioningService.bestFitPosition.x, newPose.y);
            selectController.ShowOptionsController = LogicStates.Waiting;
        }

        private IEnumerator HideIpSelectDialog()
        {
            var translation = Vector3.left * (Time.deltaTime * selectController.TransformFactor);
            var newPose = selectController.ipSelector.transform.position + translation;
            
            while (newPose.x > selectIpHomePosition.x)
            {
                selectController.ipSelector.transform.Translate(translation);
                translation = Vector3.left * (Time.deltaTime * selectController.TransformFactor);
                newPose = selectController.ipSelector.transform.position + translation;
                
                yield return null;
            }
            isDialogOpen = false;
            selectController.AddNewRobotService.IsSelectDialogOpen = isDialogOpen;
        }

        private IEnumerator UpdateListData()
        {
            foreach (var item in allIpAddresses)
            {
                item.SetActive(item.name.Contains(selectController.ElementClicked.ToString()));
                yield return null;
            }
        }

        private void ServerFailure(bool state)
        {
            gridItem.SetActive(!state);
            buttonText.text = state ? "Close" : "Save";
            serverError.SetActive(state);
        }

        private IEnumerator HandleDataRefresh()
        {
            var time = 0f;
            
            serverError.SetActive(false);
            selectController.HexSpinner.SetActive(true);
            while (selectController.DataStorage.loadingSpinner.Any(spinner => spinner.Value))
            {
                if (time > selectController.DataStorage.animationTimeout) break;
                time += Time.deltaTime;
                
                yield return null;
            }
            selectController.HexSpinner.SetActive(false);
            serverError.SetActive(true);
            
            if (selectController.DataStorage.availableRobotsNames.Count == 0) yield break;
            ServerFailure(false);
            InitListLogic();
        }

        private IEnumerator ResetStates()
        {
            selectController.StylingService.MarkAsUnselected(allIpAddresses);
            selectController.AddNewRobotService.ResetSelectState = false;
            yield return null;
        }

        private void MakeListEntry(string content, string alias, GameObject obj = null)
        {
            var newItem = obj ? obj : Instantiate(gridItem, grid.transform, false);
            newItem.transform.Find("RobotIp").GetComponent<TMP_Text>().text = content;
            newItem.name = alias;

            if (string.IsNullOrWhiteSpace(content))
            {
                newItem.transform.GetComponent<Image>().color = Color.clear;
            }
            else
            {
                newItem.transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    selectController.StylingService.MarkAsUnselectedWithCondition(allIpAddresses, 
                        selectController.ElementClicked);
                    switch (selectController.ElementClicked)
                    {
                        case ButtonType.IpAddress:
                            chosenIpText.text = content;
                            break;
                        case ButtonType.Category:
                        {
                            chosenCategoryText.text = content;
                            break;
                        }
                        case ButtonType.RobotName:
                            chosenNameText.text = content;
                            break;
                        default:
                            throw new NotSupportedException();
                    }

                    newItem.transform.GetComponent<Image>().sprite = selectController.StylingService.SelectedSprite;
                });
            }
            
            allIpAddresses.Add(newItem);
        }
    }
}
