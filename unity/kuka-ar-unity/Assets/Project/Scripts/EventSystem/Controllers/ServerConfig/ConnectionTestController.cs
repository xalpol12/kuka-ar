using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionTestController : MonoBehaviour
{
    public GameObject connectionTestComponent;
    private IpValidationService validationService;
    private ServerHttpService httpService;
   
    void Start()
    {
        validationService = IpValidationService.Instance;
        httpService = ServerHttpService.Instance;
        
        connectionTestComponent.transform
            .Find("TestConnection").GetComponent<Button>().onClick.AddListener(TestConnection);
    }

    private void TestConnection()
    {
        var ip = connectionTestComponent.transform.parent.Find("IpInputBox").GetComponent<TMP_InputField>().text;
        validationService.IpAddressValidation(ip);
        if (validationService.ValidationResult)
        {
            Debug.Log(ip);
            StartCoroutine(httpService.PingOperation(ip));
        }
    }
}
