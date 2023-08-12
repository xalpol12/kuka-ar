using Project.Scripts.Connectivity.Temp;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class AddRobotBehavior : MonoBehaviour
{
    [SerializeField]
    private float pullAddMenuMaxHeight = 0.018f;
    
    private AddRobotController robotController;
    private BottomNavController bottomNavController;
    private AddRobotRequest request;
    private GameObject selectOptions;
    private Vector3 homePosition;
    
    private bool isDialogFullyOpen;
    void Start()
    {
        robotController = GetComponent<AddRobotController>();
        bottomNavController = FindObjectOfType<BottomNavController>();
        
        // ipAddress = new InputValidation
        // {
        //     InputField = robotController.addDialog.transform.Find("IpInput").GetComponent<TMP_InputField>(),
        //     Image = robotController.addDialog.transform.Find("IpInput").GetComponent<Image>(),
        //     Touched = false,
        //     Valid = false,
        // };
        //
        // robotName = new InputValidation
        // {
        //     InputField = robotController.addDialog.transform.Find("NameInput").GetComponent<TMP_InputField>(),
        //     Image = robotController.addDialog.transform.Find("NameInput").GetComponent<Image>(),
        //     Touched = false,
        //     Valid = false,
        // };
        
        homePosition = robotController.addDialog.transform.position;
        
        isDialogFullyOpen = false;
        
        robotController.addDialog.SetActive(robotController.ShowAddDialog);
    }
    
    void Update()
    {
        if (robotController.ShowAddDialog)
        {
            if (robotController.IsSliderHold)
            {
                DragSlider();
            }
            
            ShowAddDialog();
            
            if (isDialogFullyOpen)
            {
                CollectUserInputData();
            }
            
            bottomNavController.IsDocked = false;
        }
        else
        {
            HideAddDialog();
            bottomNavController.IsDocked = true;
        }
    }

    private void ShowAddDialog()
    {
        robotController.addDialog.SetActive(true);
        
        var translation = Vector3.up * (Time.deltaTime * robotController.TransformFactor);
        var newPose = robotController.addDialog.transform.position + translation;

        if (newPose.y > 40)
        {
            translation = new Vector3();
            isDialogFullyOpen = true;
        }

        if (newPose.y > -590)
        {
            robotController.bottomNav.SetActive(false);
        }
        robotController.addDialog.transform.Translate(translation);
    }

    private void HideAddDialog()
    {
        var translation = Vector3.down * (Time.deltaTime * robotController.TransformFactor);
        var newPose = robotController.addDialog.transform.position + translation;

        if (newPose.y < homePosition.y)
        {
            translation = new Vector3();
        }

        if (newPose.y < -590)
        {
            robotController.bottomNav.SetActive(true);
        }
        
        isDialogFullyOpen = false;
        robotController.addDialog.transform.Translate(translation);
    }

    private void CollectUserInputData()
    {
        // if (!ipAddress.InputField.isFocused && ipAddress.Touched)
        // {
        //     ipAddress = validator.IpAddressValidation(ipAddress);
        // }
        //
        // if (!robotName.InputField.isFocused && robotName.Touched)
        // {
        //     robotName = validator.NameValidation(robotName);
        // }
        //
        // if (ipAddress.InputField.isFocused)
        // {
        //     ipAddress.Touched = true;
        // }
        //
        // if (robotName.InputField.isFocused)
        // {
        //     robotName.Touched = true;
        // }
        //
        // if (ipAddress.Valid && robotName.Valid)
        // {
        //     robotController.Request = new AddRobotRequest
        //     {
        //         IpAddress = ipAddress.InputField.text,
        //         RobotName = robotName.InputField.text
        //     };
        //     robotController.CanSend = true;
        // }
    }
    
    private void DragSlider()
    {
        var menuPosition = Vector3.up ;
        menuPosition.y *= Input.mousePosition.y - (Screen.height * 0.018f - homePosition.y);
        menuPosition.x = homePosition.x;
        if (menuPosition.y > Screen.height * pullAddMenuMaxHeight)
        {
            menuPosition.y = Screen.height * pullAddMenuMaxHeight;
        }

        if (menuPosition.y < homePosition.y * 0.8f)
        {
            menuPosition.y = homePosition.y * 0.8f;
            robotController.ShowAddDialog = false;
        }
        robotController.addDialog.transform.position = menuPosition;
    }
}
