using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionTestController : MonoBehaviour
{
    public GameObject connectionTestComponent;
    private IpValidationService validationService;
    private ServerHttpService httpService;
    private Image cloudIcon;
    private Sprite pingSuccessIcon;
    private Sprite pingFailedIcon;    
    
    void Start()
    {
        validationService = IpValidationService.Instance;
        httpService = ServerHttpService.Instance;
        cloudIcon = connectionTestComponent.transform.Find("Cloud").GetComponent<Image>();

        pingSuccessIcon = Resources.Load<Sprite>("Icons/cloudSuccessIcon");
        pingFailedIcon = Resources.Load<Sprite>("Icons/cloudFailedIcon");
        
        connectionTestComponent.transform
            .Find("TestConnection").GetComponent<Button>().onClick.AddListener(TestConnection);
    }

    private void TestConnection()
    {
        var ip = connectionTestComponent.transform.parent.Find("IpInputBox").GetComponent<TMP_InputField>().text;
        validationService.IpAddressValidation(ip);
        if (validationService.ValidationResult)
        {
            Debug.Log("IP: " + ip);
            StartCoroutine(httpService.PingOperation(ip));
            cloudIcon.sprite = httpService.PingResponse ? pingSuccessIcon : pingFailedIcon;
        }
    }
}
