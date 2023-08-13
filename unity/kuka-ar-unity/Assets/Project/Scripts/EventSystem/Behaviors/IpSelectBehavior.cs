using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IpSelectBehavior : MonoBehaviour
{
    private IpSelectController selectController;
    private List<GameObject> allIpAddresses;
    private Vector3 selectIpHomePosition;
    private void Start()
    {
        selectController = GetComponent<IpSelectController>();
        allIpAddresses = new List<GameObject>();
        InitListLogic();

        selectIpHomePosition = selectController.ipSelector.transform.position;
    }

    private void Update()
    {
        if (selectController.ShowOptions)
        {
            ShowIpSelectDialog();
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
        gridItem.transform.Find("RobotIp").GetComponent<TMP_Text>().text =
            selectController.HttpService.ConfiguredRobots[0].IpAddress;
        gridItem.transform.GetComponent<Button>().onClick.AddListener(() =>
        {
            selectController.StylingService.MarkAsUnselected(allIpAddresses);
            OnIpSelect(parentComponent,0);
            gridItem.transform.GetComponent<Image>().sprite = selectController.StylingService.selectedSprite;
        });
        allIpAddresses.Add(gridItem);
        
        for (var i = 1; i < selectController.HttpService.ConfiguredRobots.Count - 1; i++)
        {
            var newIpAddress = Instantiate(gridItem, grid.transform, false);
            newIpAddress.transform.Find("RobotIp").GetComponent<TMP_Text>().text =
                selectController.HttpService.ConfiguredRobots[i].IpAddress;
            newIpAddress.transform.GetComponent<Button>().onClick.AddListener(() =>
            {
                selectController.StylingService.MarkAsUnselected(allIpAddresses);
                OnIpSelect(parentComponent, newIpAddress.transform.GetSiblingIndex());
                newIpAddress.transform.GetComponent<Image>().sprite = selectController.StylingService.selectedSprite;
            });
            
            allIpAddresses.Add(newIpAddress);
        }
    }

    private void ShowIpSelectDialog()
    {
        var translation = Vector3.right * (Time.deltaTime * selectController.TransformFactor);
        var newPose = selectController.ipSelector.transform.position + translation;

        if (newPose.x > selectIpHomePosition.x + 1165)
        {
            translation = new Vector3();
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
        var http = selectController.HttpService.ConfiguredRobots[index];
        
        parent.Find("IpAddress").GetComponent<RectTransform>().gameObject.transform
            .Find("Label").GetComponent<TMP_Text>().text = http.IpAddress;
        parent.Find("ChosenCategory").GetComponent<RectTransform>().gameObject.transform
            .Find("CategoryLabel").GetComponent<TMP_Text>().text = http.RobotCategory;
        parent.Find("RobotName").GetComponent<RectTransform>().gameObject.transform
            .Find("NameLabel").GetComponent<TMP_Text>().text = http.RobotName;
    }
}
