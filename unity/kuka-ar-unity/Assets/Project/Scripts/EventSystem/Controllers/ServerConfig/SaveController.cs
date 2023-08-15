using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveController : MonoBehaviour
{
    public int id;
    public GameObject ipInputField;
    private IpValidationService validationService;
    void Start()
    {
        validationService = IpValidationService.Instance;
        
        var inputTextBox = ipInputField.GetComponent<TMP_InputField>();
        
        ipInputField.transform.parent.Find("SaveButton").GetComponent<Button>().onClick.AddListener(ValidateIp);
        inputTextBox.onValueChanged.AddListener(ResetInvalidState);
        inputTextBox.onSelect.AddListener(ClearPlaceholder);
    }


    private void ResetInvalidState(string arg)
    {
        ipInputField.GetComponent<Image>().sprite = validationService.Default();
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

    private void ClearPlaceholder(string arg)
    {
        ipInputField.transform.Find("Text Area").GetComponent<RectTransform>()
            .Find("Placeholder").GetComponent<TMP_Text>().gameObject.SetActive(false);
    }
}
