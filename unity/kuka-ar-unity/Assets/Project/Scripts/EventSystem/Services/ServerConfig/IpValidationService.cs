using System.Text.RegularExpressions;
using UnityEngine;

public class IpValidationService : MonoBehaviour
{
    public static IpValidationService Instance;
    internal bool ValidationResult;
    private Sprite valid;
    private Sprite invalid;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        valid = Resources.Load<Sprite>("Gradients/GreyishGradientSmall");
        invalid = Resources.Load<Sprite>("Gradients/GreyishGradientInvalid");
    }

    public Sprite IpAddressValidation(string validation)
    {
        var match = Regex.Match(validation,
            @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$",
            RegexOptions.Singleline);
        ValidationResult = match.Success;
        
        return match.Success ? valid : invalid;
    }

    public Sprite Default()
    {
        return valid;
    }
}
