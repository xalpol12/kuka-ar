using UnityEngine;
using UnityEngine.UI;

public class MoreOptionsController : MonoBehaviour
{
    public GameObject moreOptions;
    internal GameObject InternetText;
    internal Button BugReportButton;
    internal HttpService HttpService;
    private void Start()
    {
        HttpService = HttpService.Instance;
        
        InternetText = moreOptions.transform.parent.Find("InternetLabel").GetComponent<RectTransform>().gameObject;
        BugReportButton = moreOptions.GetComponent<Button>();
    }
}
