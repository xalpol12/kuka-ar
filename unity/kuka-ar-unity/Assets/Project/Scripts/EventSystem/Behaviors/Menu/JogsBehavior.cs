using System.Collections;
using Project.Scripts.EventSystem.Enums;
using UnityEngine;

namespace Project.Scripts.EventSystem.Behaviors.Menu
{
    public class JogsBehavior : MonoBehaviour
    {
        private JogsController jogsController;
        private GameObject jogsValues;
        private GameObject jogsDisplay;
    
        private Vector3 jogsHomePosition;
    
    private int distance;
    private void Start()
    {
        jogsController = GetComponent<JogsController>();
        jogsValues = jogsController.jogs.GetComponent<RectTransform>().Find("JogsValues").gameObject;
        jogsDisplay = jogsController.jogs.GetComponent<RectTransform>().Find("JogDisplay").gameObject;
        
        jogsDisplay.SetActive(true);
        jogsValues.SetActive(false);
        jogsHomePosition = jogsDisplay.transform.position;
        
            distance = (int)((Screen.height * 0.115f) + PositioningService.Instance.PositioningError);
        }
    
    private void Update()
    {
        if (jogsController.JogsTrigger == LogicStates.Running &&
            jogsController.Service.IsBottomNavDocked &&
            !jogsController.Service.IsAddRobotDialogOpen)
        {
            StartCoroutine(ShowJogs());
        }
        else if (jogsController.JogsTrigger == LogicStates.Hiding)
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
            var toggleActive = false;
            foreach (Transform child in jogsValues.transform)
            {
                Vector3 translation;
                if (child.name == "HideJogs")
                {
                    toggleActive = true;
                    yield return null;

                    jogsController.JogsTrigger = LogicStates.Waiting; 
                    break;
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
                translation = Vector3.up * 
                              (jogsController.transformFactor * (Time.deltaTime * 
                                                                 (child.GetSiblingIndex() - 1) * distance));
                var newPosition = child.position + translation;
                if (newPosition.y - 10 > jogsHomePosition.y)
                {
                    yield return null;
                    
                    jogsController.JogsTrigger = LogicStates.Waiting;
                    break;
                }  
            }
        
        }

        private void ShowJogs()
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
                Vector3 translation;
                if (child.name == "HideJogs")
                {
                    jogsController.JogsTrigger = LogicStates.Waiting; 

                    yield break;
                }
                else
                {
                    jogsController.JogsTrigger = LogicStates.Waiting; 

                    yield break;
                }
            
                child.Translate(translation);
            }
        }
    }
}
