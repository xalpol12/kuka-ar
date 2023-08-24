using Project.Scripts.EventSystem.Services.Menu;
using Project.Scripts.EventSystem.Services.ServerConfig;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.EventSystem.Controllers.ServerConfig
{
    public class SaveController : MonoBehaviour
    {
        [SerializeField] private GameObject ipInputField;
        private IpValidationService validationService;
        private HttpService httpService;
        private Sprite cloudIcon;
        void Start()
        {
            validationService = IpValidationService.Instance;
            httpService = HttpService.Instance;
            cloudIcon = Resources.Load<Sprite>("Icons/cloudIcon");

            var inputTextBox = ipInputField.GetComponent<TMP_InputField>();
            inputTextBox.lineType = TMP_InputField.LineType.SingleLine;

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
                httpService.ConfiguredIp = ipInputField.GetComponent<TMP_InputField>().text;
            }
            httpService.OnClickDataReload(4);
        }

        private void ClearPlaceholder(string arg)
        {
            ipInputField.transform.Find("Text Area").GetComponent<RectTransform>()
                .Find("Placeholder").GetComponent<TMP_Text>().gameObject.SetActive(false);
        }
    }
}
