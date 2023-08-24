using Project.Scripts.Connectivity.Models.AggregationClasses;
using TMPro;
using UnityEngine;

public class AddNewRobotService : MonoBehaviour
{
    public static AddNewRobotService Instance;
    public GameObject parent;
    internal bool ResetSelectState;
    private TMP_Text ip;
    private TMP_Text category;
    private TMP_Text robotName;
    private AddRobotData defaultValues;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ResetSelectState = false;

        ip = parent.transform.Find("IpAddress").GetComponent<RectTransform>().gameObject.transform
            .Find("Label").GetComponent<TMP_Text>();
        category = parent.transform.Find("ChosenCategory").GetComponent<RectTransform>().gameObject
            .transform.Find("CategoryLabel").GetComponent<TMP_Text>();
        robotName = parent.transform.Find("RobotName").GetComponent<RectTransform>().gameObject.transform
            .Find("NameLabel").GetComponent<TMP_Text>();
        
        defaultValues = new AddRobotData
        {
            IpAddress = "IP Address",
            RobotCategory = "Category",
            RobotName = "Name"
        };
    }

    private void Update()
    {
        if (!ResetSelectState) return;
        ip.text = defaultValues.IpAddress;
        category.text = defaultValues.RobotCategory;
        robotName.text = defaultValues.RobotName;
        ResetSelectState = false;
    }
}
