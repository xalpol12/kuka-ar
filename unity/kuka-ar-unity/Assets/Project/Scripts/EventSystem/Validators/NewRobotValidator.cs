using System.Text.RegularExpressions;
using Project.Scripts.Connectivity.Temp;
using UnityEngine;

public class NewRobotValidator : MonoBehaviour
{   
    [SerializeField]
    private Sprite valid;
    
    [SerializeField]
    private Sprite invalid;
    
    public InputValidation IpAddressValidation(InputValidation validation)
    {
        var match = Regex.Match(validation.InputField.text,
            @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$",
            RegexOptions.Singleline);
        if (match.Success)
        {
            validation.Image.sprite = valid;
            validation.Valid = true;
        }
        else
        {
            validation.Image.sprite = invalid;
        }
        
        return validation;
    }

    public InputValidation NameValidation(InputValidation validation)
    {
        if (string.IsNullOrWhiteSpace(validation.InputField.text))
        {
            validation.Image.sprite = invalid;
        }
        else
        {
            validation.Image.sprite = valid;
            validation.Valid = true;
        }

        return validation;
    }
}
