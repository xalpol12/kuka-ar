using System.Collections.Generic;
using Project.Scripts.Connectivity.Enums;
using Project.Scripts.Connectivity.Models.AggregationClasses;
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
        private List<GameObject> allIpAddresses;
        private Vector3 selectIpHomePosition;
        private AnimationStates state;
        private void Start()
        {
            selectController = GetComponent<IpSelectController>();
            allIpAddresses = new List<GameObject>();
            state = AnimationStates.FadeIn;

            InitListLogic();

            selectIpHomePosition = selectController.ipSelector.transform.position;
            selectController.ipSelector.transform.Find("IpAddressList").GetComponent<RectTransform>()
                .transform.Find("IpAddressGrid").GetComponent<RectTransform>().gameObject
                .transform.Find("ServerError").GetComponent<RectTransform>().gameObject
                .transform.Find("TryAgain").GetComponent<Button>().onClick.AddListener(() =>
                {
                    selectController.HttpService.OnClickDataReload(4);
                    if (selectController.HttpService.ConfiguredRobots.Count > 0)
                    {
                        ServerFailure(false);
                        InitListLogic();
                    }
                });
        }

        private void Update()
        {
            if (selectController.ShowOptions)
            {
                UpdateListData();
                ShowIpSelectDialog();
                if (selectController.AddNewRobotService.ResetSelectState)
                {
                    selectController.StylingService.MarkAsUnselected(allIpAddresses);
                    selectController.AddNewRobotService.ResetSelectState = false;
                }
            }
            else
            {
                HideIpSelectDialog();
            }
        }

        private void InitListLogic()
        {
            var parentComponent = selectController.ipSelector.transform.parent;
            var ipList = selectController.ipSelector.transform.Find("IpAddressList").GetComponent<RectTransform>();
            var grid = ipList.transform.Find("IpAddressGrid").GetComponent<RectTransform>().gameObject;
            var gridItem = grid.transform.Find("IpAddressGridElement").GetComponent<RectTransform>().gameObject;

            if (selectController.HttpService.ConfiguredRobots.Count == 0)
            {
                ServerFailure(true);
                return;
            }

            gridItem.transform.Find("RobotIp").GetComponent<TMP_Text>().text =
                selectController.HttpService.ConfiguredRobots[0].IpAddress;
            gridItem.transform.GetComponent<Button>().onClick.AddListener(() =>
            {
                selectController.StylingService.MarkAsUnselected(allIpAddresses);
                OnIpSelect(parentComponent,gridItem.transform.GetSiblingIndex());
                gridItem.transform.GetComponent<Image>().sprite = selectController.StylingService.SelectedSprite;
            });
            allIpAddresses.Add(gridItem);

            for (var i = 1; i < selectController.HttpService.ConfiguredRobots.Count + 2; i++)
            {
                var newIpAddress = Instantiate(gridItem, grid.transform, false);
                if (i > selectController.HttpService.ConfiguredRobots.Count - 1)
                {
                    newIpAddress.transform.GetComponent<Image>().color = Color.clear;
                }
                else
                {
                    newIpAddress.transform.Find("RobotIp").GetComponent<TMP_Text>().text =
                                    selectController.HttpService.ConfiguredRobots[i].IpAddress;
                }

                newIpAddress.transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    selectController.StylingService.MarkAsUnselected(allIpAddresses);
                    if (gridItem.transform.GetSiblingIndex() > selectController.HttpService.ConfiguredRobots.Count + 1)
                    {
                        return;
                    }
                    OnIpSelect(parentComponent, newIpAddress.transform.GetSiblingIndex() - 1);
                    newIpAddress.transform.GetComponent<Image>().sprite = selectController.StylingService.SelectedSprite;
                });

                allIpAddresses.Add(newIpAddress);
            }
        }

        private void ShowIpSelectDialog()
        {
            var translation = Vector3.right * (Time.deltaTime * selectController.TransformFactor);
            var newPose = selectController.ipSelector.transform.position + translation;

            if (newPose.x > selectController.PositioningService.BestFitPosition.x)
            {
                if (state == AnimationStates.FadeIn)
                {
                    var finalPose = new Vector3(selectController.PositioningService.BestFitPosition.x, newPose.y);

                    selectController.ipSelector.transform.position = finalPose;
                    state = AnimationStates.StandBy;
                }
                return;
            }

            selectController.ipSelector.transform.Translate(translation);
        }

        private void HideIpSelectDialog()
        {
            var translation = Vector3.left * (Time.deltaTime * selectController.TransformFactor);
            var newPose = selectController.ipSelector.transform.position + translation;

            if (newPose.x < selectIpHomePosition.x)
            {
                translation = new Vector3();
            }

            selectController.ipSelector.transform.Translate(translation);
        }

        private void OnIpSelect(Transform parent, int index)
        {
            var http = new AddRobotData();
            if (index < selectController.HttpService.ConfiguredRobots.Count)
            {
                http = selectController.HttpService.ConfiguredRobots[index];
            }

            switch (selectController.ElementClicked)
            {
                case ButtonType.IpAddress:
                    parent.Find("IpAddress").GetComponent<RectTransform>().gameObject.transform
                                .Find("Label").GetComponent<TMP_Text>().text = http.IpAddress;
                    break;
                case ButtonType.Category:
                    var mod = index % selectController.HttpService.CategoryNames.Count;
                    parent.Find("ChosenCategory").GetComponent<RectTransform>().gameObject.transform
                            .Find("CategoryLabel").GetComponent<TMP_Text>().text =
                        selectController.HttpService.CategoryNames[mod];
                    break;
                case ButtonType.RobotName:
                    parent.Find("RobotName").GetComponent<RectTransform>().gameObject.transform
                        .Find("NameLabel").GetComponent<TMP_Text>().text = http.RobotName;
                    break;
            }
        }

        private void UpdateListData()
        {
            var parentComponent = selectController.ipSelector.transform.parent;
            if (selectController.PrevElementClicked != selectController.ElementClicked)
            {
                selectController.StylingService.MarkAsUnselected(allIpAddresses);
            }

            foreach (var (item, index) in allIpAddresses.WithIndex())
            {
                var temp = "";
                var currentText = item.transform.Find("RobotIp").GetComponent<TMP_Text>().text;
                if (currentText == parentComponent.Find("IpAddress").GetComponent<RectTransform>()
                        .gameObject.transform.Find("Label").GetComponent<TMP_Text>().text ||
                    currentText == parentComponent.Find("ChosenCategory").GetComponent<RectTransform>()
                        .gameObject.transform.Find("CategoryLabel").GetComponent<TMP_Text>().text ||
                    currentText == parentComponent.Find("RobotName").GetComponent<RectTransform>()
                        .gameObject.transform.Find("NameLabel").GetComponent<TMP_Text>().text)
                {
                    item.transform.GetComponent<Image>().sprite = selectController.StylingService.SelectedSprite;
                }
                if (selectController.ElementClicked == ButtonType.Category &&
                    index > selectController.HttpService.CategoryNames.Count - 1)
                {
                    temp = null;
                }
                else
                {
                    if (index > selectController.HttpService.ConfiguredRobots.Count - 1)
                    {
                        temp = "";
                    }
                    else
                    {
                        switch (selectController.ElementClicked)
                        {
                            case ButtonType.IpAddress:
                                temp = selectController.HttpService.ConfiguredRobots[index].IpAddress;
                                break;
                            case ButtonType.Category:
                                temp = selectController.HttpService.CategoryNames[index];
                                break;
                            case ButtonType.RobotName:
                                temp = selectController.HttpService.ConfiguredRobots[index].RobotName;
                                break;
                        }
                    }

                }

                if (temp == null)
                {
                    item.gameObject.SetActive(false);
                }
                else
                {
                    item.gameObject.SetActive(true);
                    item.transform.Find("RobotIp").GetComponent<TMP_Text>().text = temp;
                }
            }
        }

        private void ServerFailure(bool state)
        {
            var view = selectController.ipSelector.transform.Find("IpAddressList").GetComponent<RectTransform>()
                .transform.Find("IpAddressGrid").GetComponent<RectTransform>().gameObject;
            view.transform.Find("IpAddressGridElement").GetComponent<RectTransform>().gameObject.SetActive(!state);
            selectController.ipSelector.transform.parent.transform.gameObject
                    .transform.Find("Button").GetComponent<Button>().gameObject
                    .transform.Find("SaveCloseButton").GetComponent<TMP_Text>().text = state ? "Close" : "Save";
            view.transform.Find("ServerError").GetComponent<RectTransform>().gameObject.SetActive(state);
        }
    }
}
