using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
        private Vector3 jogsHomePosition;
        
        private int distance;

        private readonly List<TMP_Text> jogValueLabels = new();

        private void Start()
        {
            jogsController = GetComponent<JogsController>();

            jogsValues = jogsController.gameObject.GetComponent<RectTransform>().Find("JogsValues").gameObject;
            jogsDisplay = jogsController.gameObject.GetComponent<RectTransform>().Find("JogDisplay").gameObject;
            
            jogsDisplay.SetActive(true);
            jogsValues.SetActive(false);
            jogsHomePosition = jogsDisplay.transform.position;
        
            distance = (int)((Screen.height * 0.115f) + PositioningService.PositioningError);

             foreach (Transform child in jogsValues.transform)
            {
                if (child.name != "HideJogs")
                {
                    jogValueLabels.Add(child.transform.Find("JogValue").GetComponent<TMP_Text>());
                }
            }
        }

        private void Update()
        {
            if (jogsController.JogsTrigger == LogicStates.Running &&
                jogsController.Service.IsBottomNavDocked &&
                !jogsController.Service.IsAddRobotDialogOpen)
            {
                StartCoroutine(ShowJogs());
            }
            else if (jogsController.JogsTrigger == LogicStates.Hiding || 
                     !jogsController.Service.IsBottomNavDocked || jogsController.Service.IsAddRobotDialogOpen)
            {
                StartCoroutine(HideJogs());
            }

            if (jogsController.UpdateJogs)
            {
                UpdateJogsDisplayedValues();
                jogsController.UpdateJogs = false;
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
                        jogsController.ShowJogs = false;
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
                        jogsController.ShowJogs = false;
                        break;
                    }  
                }
            
                child.Translate(translation);
            }

            if (!toggleActive) yield break;
            jogsDisplay.SetActive(true);
            jogsValues.SetActive(false);
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

        private void UpdateJogsDisplayedValues()
        {
            Span<double> temp = stackalloc double[] {
                jogsController.Joints.J1,
                jogsController.Joints.J2,
                jogsController.Joints.J3,
                jogsController.Joints.J4,
                jogsController.Joints.J5,
                jogsController.Joints.J6 };

            for (var i = 0; i < temp.Length; i++)
            {
                jogValueLabels[i].text = temp[i].ToString("F1", CultureInfo.InvariantCulture);
            }
        }
    }
}
