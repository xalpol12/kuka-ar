using UnityEngine;

public class JogsBehavior : MonoBehaviour
{
    private JogsController jogsController;
    private BottomNavController bottomNavController;
    private GameObject jogsValues;
    private GameObject jogsDisplay;
    
    private Vector3 jogsHomePosition;
    
    private int distance;
    void Start()
    {
        jogsController = GetComponent<JogsController>();
        bottomNavController = FindObjectOfType<BottomNavController>();
        jogsValues = jogsController.jogs.GetComponent<RectTransform>().Find("JogsValues").gameObject;
        jogsDisplay = jogsController.jogs.GetComponent<RectTransform>().Find("JogDisplay").gameObject;
        ;
        
        jogsDisplay.SetActive(!jogsController.ShowJogs);
        jogsValues.SetActive(jogsController.ShowJogs);
        jogsHomePosition = jogsDisplay.transform.position;
        distance = (int)(Screen.height * 0.115f);
    }
    
    void Update()
    {
        if (jogsController.ShowJogs && bottomNavController.IsDocked)
        {
            ShowJogs();
        }
        else
        {
            HideJogs();
        }
    }

    private void HideJogs()
    {
        var toggleActive = false;
        foreach (Transform child in jogsValues.transform)
        {
            Vector3 translation;
            if (child.name == "HideJogs")
            {
                translation = Vector3.down * (jogsController.transformFactor * (Time.deltaTime * distance));
                var newPosition = child.position + translation;
                if (newPosition.y < jogsHomePosition.y)
                {
                    toggleActive = true;
                    break;
                }
            }
            else
            {
                translation = Vector3.up * 
                              (jogsController.transformFactor * (Time.deltaTime * 
                                                                 (child.GetSiblingIndex() - 1) * distance));
                var newPosition = child.position + translation;
                if (newPosition.y - 10 > jogsHomePosition.y)
                {
                    break;
                }  
            }
            
            child.Translate(translation);
        }

        if (toggleActive)
        {
            jogsDisplay.SetActive(true);
            jogsValues.SetActive(false);
        }
        
    }

    private void ShowJogs()
    {
        jogsDisplay.SetActive(false);
        jogsValues.SetActive(true);
        foreach (Transform child in jogsValues.transform)
        {
            Vector3 translation;
            if (child.name == "HideJogs")
            {
                translation = Vector3.up * (jogsController.transformFactor * (Time.deltaTime * distance));
                var newPosition = child.position + translation;
                if (newPosition.y > jogsHomePosition.y + distance)
                {
                    break;
                }
            }
            else
            {
                translation = Vector3.down *
                              (jogsController.transformFactor * (Time.deltaTime * (child.GetSiblingIndex() - 1) * distance));
                var newPosition = child.position + translation;
                if (newPosition.y < jogsHomePosition.y - (child.GetSiblingIndex() - 1) * distance)
                {
                    break;
                }
            }
            
            child.Translate(translation);
        }
    }
}
