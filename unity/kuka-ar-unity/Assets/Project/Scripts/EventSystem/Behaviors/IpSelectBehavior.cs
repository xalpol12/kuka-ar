using TMPro;
using UnityEngine;

public class IpSelectBehavior : MonoBehaviour
{
    private IpSelectController selectController;
    private Vector3 selectIpHomePosition;
    private void Start()
    {
        selectController = GetComponent<IpSelectController>();
        var ipList = selectController.ipSelector.transform.Find("IpAddressList").GetComponent<RectTransform>();
        var grid = ipList.transform.Find("IpAddressGrid").GetComponent<RectTransform>().gameObject;
        var gridItem = grid.transform.Find("IpAddressGridElement").GetComponent<RectTransform>().gameObject;

        for (var i = 0; i < 25; i++)
        {
            var newIpAddress = Instantiate(gridItem, grid.transform, false);
            newIpAddress.transform.Find("RobotIp").GetComponent<TMP_Text>().text = "192.168.168.1" + i;
        }
        
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
}
