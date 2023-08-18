using UnityEngine;

public class AddRobotBehavior : MonoBehaviour
{
    [SerializeField]
    private float pullAddMenuMaxHeight = 0.018f;

    [SerializeField] private float errorMargin = 0.05f;
    private AddRobotController robotController;
    private JogsControlService service;
    private GameObject selectOptions;
    private Vector3 homePosition;
    
    private bool isDialogFullyOpen;
    void Start()
    {
        robotController = GetComponent<AddRobotController>();
        service = FindObjectOfType<JogsControlService>();
        selectOptions = robotController.addDialog.transform.Find("SelectOptions")
            .GetComponent<RectTransform>().gameObject;
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

            selectOptions.SetActive(isDialogFullyOpen);
            service.IsAddRobotDialogOpen = true;
        }
        else
        {
            HideAddDialog();
            service.IsAddRobotDialogOpen = false;
        }
    }

    private void ShowAddDialog()
    {
        robotController.addDialog.SetActive(true);
        
        var translation = Vector3.up * (Time.deltaTime * robotController.TransformFactor);
        var newPose = robotController.addDialog.transform.position + translation;

        if (newPose.y > Screen.width * errorMargin)
        {
            translation = new Vector3();
            isDialogFullyOpen = true;
        }

        if (newPose.y > homePosition.y / 2)
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

        if (newPose.y < homePosition.y / 2 )
        {
            robotController.bottomNav.SetActive(true);
        }
        
        isDialogFullyOpen = false;
        robotController.addDialog.transform.Translate(translation);
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
            menuPosition.y = homePosition.y * 0.83f;
            robotController.ShowAddDialog = false;
        }
        robotController.addDialog.transform.position = menuPosition;
    }
}
