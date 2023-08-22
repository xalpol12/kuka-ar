using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JogsBehavior : MonoBehaviour
{
    private JogsController jogsController;
    private GameObject jogsValues;
    private GameObject jogsDisplay;
    
    private Vector3 jogsHomePosition;
    
    private int distance;
    void Start()
    {
        jogsController = GetComponent<JogsController>();
        jogsValues = jogsController.jogs.GetComponent<RectTransform>().Find("JogsValues").gameObject;
        jogsDisplay = jogsController.jogs.GetComponent<RectTransform>().Find("JogDisplay").gameObject;
        
        jogsDisplay.SetActive(!jogsController.ShowJogs);
        jogsValues.SetActive(jogsController.ShowJogs);
        jogsHomePosition = jogsDisplay.transform.position;
        
        distance = (int)((Screen.height * 0.115f) + PositioningService.Instance.PositioningError);
    }
    
    void Update()
    {
        if (jogsController.ShowJogs &&
            jogsController.Service.IsBottomNavDocked &&
            !jogsController.Service.IsAddRobotDialogOpen)
        {
            StartCoroutine(ShowJogs());
        }
        else
        {
            StartCoroutine(HideJogs());
        }
    }

    private IEnumerator HideJogs()
    {
        yield return null;
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
                    yield return null;
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
                    yield return null;
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

    private IEnumerator ShowJogs()
    {
        yield return null;
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
                    yield break;
                }
            }
            else
            {
                translation = Vector3.down *
                              (jogsController.transformFactor * (Time.deltaTime * (child.GetSiblingIndex() - 1) * distance));
                var newPosition = child.position + translation;
                if (newPosition.y < jogsHomePosition.y - (child.GetSiblingIndex() - 1) * distance)
                {
                    yield break;
                }
            }
            
            child.Translate(translation);
        }
    }
}
