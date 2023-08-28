using Project.Scripts.Connectivity.Http;
using Project.Scripts.Connectivity.Http.Requests;
using Project.Scripts.EventSystem.Enums;
using Project.Scripts.EventSystem.Services.Menu;
using Project.Scripts.EventSystem.Services.ServerConfig;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using HttpClientWrapper = Project.Scripts.Connectivity.Http.HttpClientWrapper;

namespace Project.Scripts.EventSystem.Controllers.ServerConfig
{
    public class SaveController : MonoBehaviour
    {
        [SerializeField] private GameObject ipInputField;
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
            if (PlayerPrefs.GetInt("firstRun") != new PlayersPrefsStates().FirstRun)
            {
                inputTextBox.text = PlayerPrefs.GetString("serverIp");
            }
        
            ipInputField.transform.parent.Find("SaveButton").GetComponent<Button>().onClick.AddListener(ValidateIp);
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

        private void ValidateIp()
        {
            ipInputField.GetComponent<Image>().sprite =
                validationService.IpAddressValidation(ipInputField.GetComponent<TMP_InputField>().text);
        }

        private void UpdateState(string arg)
        {
            validationService.IpAddressValidation(ipInputField.GetComponent<TMP_InputField>().text);

            if (validationService.ValidationResult)
            {
                httpClient.BaseAddress = ipInputField.GetComponent<TMP_InputField>().text;
            }
            InvokeAllEndpoints();
        }

        private void InvokeAllEndpoints()
        {
            httpClient.ExecuteRequest(new GetTargetImagesRequest());
            httpClient.ExecuteRequest(new GetRobotConfigDataRequest());
            httpClient.ExecuteRequest(new GetSavedRobotsRequest());
        }

        private void ClearPlaceholder(string arg)
        {
            ipInputField.transform.Find("Text Area").GetComponent<RectTransform>()
                .Find("Placeholder").GetComponent<TMP_Text>().gameObject.SetActive(false);
        }
    }
}
