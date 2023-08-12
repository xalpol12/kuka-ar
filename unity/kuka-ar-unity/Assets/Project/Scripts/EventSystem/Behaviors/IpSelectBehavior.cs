using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IpSelectBehavior : MonoBehaviour
{
    private IpSelectController selectController;
    private SelectableStylingService stylingService;
    private HttpService httpService;
    private List<GameObject> allIpAddresses;
    private Vector3 selectIpHomePosition;
    private void Start()
    {
        selectController = GetComponent<IpSelectController>();
        stylingService = FindObjectOfType<SelectableStylingService>();
        httpService = FindObjectOfType<HttpService>();
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
        gridItem.transform.GetComponent<Button>().onClick.AddListener(() =>
        {
            stylingService.MarkAsUnselected(allIpAddresses);
            FillRequest(gridItem.transform.Find("RobotIp").GetComponent<TMP_Text>().text);
            OnIpSelect(parentComponent);
            gridItem.transform.GetComponent<Image>().sprite = stylingService.selectedSprite;
        });
        
        for (var i = 0; i < 25; i++)
        {
            var newIpAddress = Instantiate(gridItem, grid.transform, false);
            var ip = newIpAddress.transform.Find("RobotIp").GetComponent<TMP_Text>().text = "192.168.168.1" + i;
            newIpAddress.transform.GetComponent<Button>().onClick.AddListener(() =>
            {
                stylingService.MarkAsUnselected(allIpAddresses);
                FillRequest(ip);
                OnIpSelect(parentComponent);
                newIpAddress.transform.GetComponent<Image>().sprite = stylingService.selectedSprite;
            });
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

    private void OnIpSelect(Transform parent)
    {
        parent.Find("IpAddress").GetComponent<RectTransform>().gameObject.transform
            .Find("Label").GetComponent<TMP_Text>().text = selectController.Request.IpAddress;
        parent.Find("ChosenCategory").GetComponent<RectTransform>().gameObject.transform
            .Find("CategoryLabel").GetComponent<TMP_Text>().text = selectController.Request.RobotCategory;
        parent.Find("RobotName").GetComponent<RectTransform>().gameObject.transform
            .Find("NameLabel").GetComponent<TMP_Text>().text = selectController.Request.RobotName;
    }

    private void FillRequest(string ip)
    {
        // TODO CALL TO BACKEND
        var robotName = "KRL XXXYYY";
        var category = "KRL C20";
        selectController.Request.IpAddress = ip;
        selectController.Request.RobotName = robotName;
        selectController.Request.RobotCategory = category;
    }
}
