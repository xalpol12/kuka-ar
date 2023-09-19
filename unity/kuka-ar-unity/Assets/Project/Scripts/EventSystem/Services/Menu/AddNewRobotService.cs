using Project.Scripts.Connectivity.Models.AggregationClasses;
using TMPro;
using UnityEngine;

namespace Project.Scripts.EventSystem.Services.Menu
{
    public class AddNewRobotService : MonoBehaviour
    {
        public static AddNewRobotService Instance;
        public GameObject parent;
        public bool ResetSelectState;
        public bool IsSelectDialogOpen;
        private TMP_Text ip;
        private TMP_Text category;
        private TMP_Text robotName;
        private Robot defaultValues;
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ResetSelectState = false;
            IsSelectDialogOpen = false;

            ip = parent.transform.Find("IpAddress").GetComponent<RectTransform>().gameObject.transform
                .Find("Label").GetComponent<TMP_Text>();
            category = parent.transform.Find("ChosenCategory").GetComponent<RectTransform>().gameObject
                .transform.Find("CategoryLabel").GetComponent<TMP_Text>();
            robotName = parent.transform.Find("RobotName").GetComponent<RectTransform>().gameObject.transform
                .Find("NameLabel").GetComponent<TMP_Text>();
        
            defaultValues = new Robot
            {
                IpAddress = "IP Address",
                Category = "Category",
                Name = "Name"
            };
        }

        private void Update()
        {
            if (!ResetSelectState) return;
            ip.text = defaultValues.IpAddress;
            category.text = defaultValues.Category;
            robotName.text = defaultValues.Name;
            ResetSelectState = false;
        }
    }
}
