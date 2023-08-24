using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionTestController : MonoBehaviour
{
    public GameObject connectionTestComponent;
    private IpValidationService validationService;
    private ServerHttpService httpService;
   
    private void Start()
    {
        validationService = IpValidationService.Instance;
        httpService = ServerHttpService.Instance;
        
        connectionTestComponent.transform
            .Find("TestConnection").GetComponent<Button>().onClick.AddListener(TestConnection);
    }

    private void TestConnection()
    {
        var parent = connectionTestComponent.transform.parent;
        var ip = parent.Find("IpInputBox").GetComponent<TMP_InputField>().text;
        parent.Find("IpInputBox").GetComponent<Image>().sprite =
            validationService.IpAddressValidation(ip);
        
        if (validationService.ValidationResult)
        {
            StartCoroutine(httpService.PingOperation(ip));
        }
    }
}
