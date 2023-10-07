using Project.Scripts.Connectivity.Http;
using Project.Scripts.Connectivity.Http.Requests;
using Project.Scripts.EventSystem.Enums;
using Project.Scripts.EventSystem.Services.ServerConfig;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.EventSystem.Controllers.ServerConfig
{
    public class SaveController : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("IP input field reference")]
        private GameObject ipInputField;
        
        private IpValidationService validationService;
        private HttpClientWrapper httpClient;
        private Sprite cloudIcon;
        private void Start()
        {
            validationService = IpValidationService.Instance;
            httpClient = HttpClientWrapper.Instance;
            cloudIcon = Resources.Load<Sprite>("Icons/cloudIcon");
        
            var inputTextBox = ipInputField.GetComponent<TMP_InputField>();
            inputTextBox.lineType = TMP_InputField.LineType.SingleLine;
            if (PlayerPrefs.GetInt("firstRun") != PlayersPrefsStates.FirstRun)
            {
                inputTextBox.text = PlayerPrefs.GetString("serverIp");
            }
        
            ipInputField.transform.parent.Find("SaveButton").GetComponent<Button>().onClick.AddListener(SaveConfigurationIp);
            inputTextBox.onValueChanged.AddListener(ResetInvalidState);
            inputTextBox.onSelect.AddListener(ClearPlaceholder);
            inputTextBox.onEndEdit.AddListener(UpdateState);
        }


        private void ResetInvalidState(string arg)
        {
            ipInputField.GetComponent<Image>().sprite = validationService.Default();
            ipInputField.transform.parent.Find("TestConnectionController").GetComponent<RectTransform>()
                .Find("Cloud").GetComponent<Image>().sprite = cloudIcon;
        
            if (string.IsNullOrWhiteSpace(ipInputField.GetComponent<TMP_InputField>().text))
            {
                ipInputField.transform.Find("Text Area").GetComponent<RectTransform>()
                    .Find("Placeholder").GetComponent<TMP_Text>().gameObject.SetActive(true);
            }
        }

        private void SaveConfigurationIp()
        {
            ipInputField.GetComponent<Image>().sprite =
                validationService.IpAddressValidation(ipInputField.GetComponent<TMP_InputField>().text);
        }

        private void UpdateState(string arg)
        {
            validationService.IpAddressValidation(ipInputField.GetComponent<TMP_InputField>().text);

            if (validationService.ValidationResult)
            {
                var ipAddress = ipInputField.GetComponent<TMP_InputField>().text;
                PlayerPrefs.SetString("serverIp", ipAddress);
                httpClient.BaseAddress = ipAddress;
            }
            ServerInvoker.Invoker.GetFullData();
        }
        
        private void ClearPlaceholder(string arg)
        {
            ipInputField.transform.Find("Text Area").GetComponent<RectTransform>()
                .Find("Placeholder").GetComponent<TMP_Text>().gameObject.SetActive(false);
        }
    }
}
