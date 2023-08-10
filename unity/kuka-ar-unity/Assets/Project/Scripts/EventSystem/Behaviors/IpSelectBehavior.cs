using UnityEngine;

public class IpSelectBehavior : MonoBehaviour
{
    private IpSelectController selectController;
    private Vector3 selectIpHomePosition;
    private void Start()
    {
        selectController = GetComponent<IpSelectController>();

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
