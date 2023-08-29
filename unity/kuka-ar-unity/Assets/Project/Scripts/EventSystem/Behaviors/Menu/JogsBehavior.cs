using System.Collections;
using System.Globalization;
using Project.Scripts.Connectivity.Models.KRLValues;
using Project.Scripts.EventSystem.Controllers.Menu;
using Project.Scripts.EventSystem.Enums;
using Project.Scripts.EventSystem.Services.Menu;
using TMPro;
using UnityEngine;

namespace Project.Scripts.EventSystem.Behaviors.Menu
{
    public class JogsBehavior : MonoBehaviour
    {
        private JogsController jogsController;
        private GameObject jogsValues;
        private GameObject jogsDisplay;
        private SelectableLogicService logicService;
    
        private Vector3 jogsHomePosition;
    
        private int distance;
        private void Start()
        {
            jogsController = GetComponent<JogsController>();
            logicService = SelectableLogicService.Instance;
            
            jogsValues = jogsController.jogs.GetComponent<RectTransform>().Find("JogsValues").gameObject;
            jogsDisplay = jogsController.jogs.GetComponent<RectTransform>().Find("JogDisplay").gameObject;
        
            jogsDisplay.SetActive(true);
            jogsValues.SetActive(false);
            jogsHomePosition = jogsDisplay.transform.position;
        
            distance = (int)((Screen.height * 0.115f) + PositioningService.Instance.PositioningError);

            jogsController.RobotsHandler.trackedRobots[logicService.SelectedIpAddress].ActiveJointsUpdated +=
                (sender,e) => UpdateJogsDisplayedValues(e);
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
                Vector3 translation;
                if (child.name == "HideJogs")
                {
                    translation = Vector3.down * (jogsController.transformFactor * (Time.deltaTime * distance));
                    var newPosition = child.position + translation;
                    if (newPosition.y < jogsHomePosition.y)
                    {
                        toggleActive = true;
                        yield return null;

                        jogsController.JogsTrigger = LogicStates.Waiting; 
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
                    
                        jogsController.JogsTrigger = LogicStates.Waiting;
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
                        jogsController.JogsTrigger = LogicStates.Waiting; 

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
                        jogsController.JogsTrigger = LogicStates.Waiting; 

                        yield break;
                    }
                }
            
                child.Translate(translation);
            }
        }

        private void UpdateJogsDisplayedValues(KRLJoints e)
        {
            var temp = new[] { e.J1, e.J2, e.J3, e.J4, e.J5, e.J6 };
            foreach (Transform child in jogsValues.transform)
            {
                if (child.name != "HideJogs" )
                {
                    child.transform.GetComponent<TMP_Text>().text =
                        temp[child.GetSiblingIndex() - 1].ToString(CultureInfo.InvariantCulture);
                }
            }
        }
    }
}
