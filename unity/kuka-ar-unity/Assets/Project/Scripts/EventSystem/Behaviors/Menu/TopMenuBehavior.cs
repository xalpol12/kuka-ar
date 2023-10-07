using System.Collections;
using Project.Scripts.EventSystem.Controllers.Menu;
using Project.Scripts.EventSystem.Enums;
using TMPro;
using UnityEngine;

namespace Project.Scripts.EventSystem.Behaviors.Menu
{
    public class TopMenuBehavior : MonoBehaviour
    {
        private TopMenuController topMenu;
        
        private TMP_Text toolNo;
        private TMP_Text baseNo;
        private Vector3 dockPosition;

        private const float Swap = 0.95f;

        private void Start()
        {
            topMenu = GetComponent<TopMenuController>();

            var topPanel = topMenu.ConstantTopPanel.transform;
            toolNo = topPanel.Find("ToolBean").GetComponent<RectTransform>().gameObject
                .transform.Find("Tool").GetComponent<TMP_Text>();
            baseNo = topPanel.Find("WorldBean").GetComponent<RectTransform>().gameObject
                .transform.Find("World").GetComponent<TMP_Text>();
            dockPosition = topMenu.CoordSelectMenu.transform.position;

        }

        private void Update()
        {
            if (topMenu.CoordListState == LogicStates.Running)
            {
                DragSlider();
            } else if (topMenu.CoordListState == LogicStates.AutoPulling)
            {
                StartCoroutine(AutoPull());
                topMenu.CoordListState = LogicStates.Waiting;
            }
            
            if (topMenu.ConstantPanelState == LogicStates.Running)
            {
                StartCoroutine(DropTopMenu());
                topMenu.ConstantPanelState = LogicStates.Waiting;
            }
        }

        private IEnumerator DropTopMenu()
        {
            topMenu.ConstantTopPanel.SetActive(false);
            topMenu.jogs.SetActive(false);
            
            var translation = Vector3.down * (Time.deltaTime * topMenu.transformFactor);
            var menuPosition = topMenu.CoordSelectMenu.transform.position + translation;

            while (menuPosition.y > Screen.height * topMenu.dropScreenHeight)
            {
                topMenu.CoordSelectMenu.transform.Translate(translation);
                menuPosition = topMenu.CoordSelectMenu.transform.position + translation;
                yield return null;
            }

            topMenu.CoordSelectMenu.transform.position = menuPosition;
        }

        private void DragSlider()
        {
            var menuPosition = new Vector3(dockPosition.x ,Input.mousePosition.y);
            if (menuPosition.y < Screen.height * topMenu.dropScreenHeight)
            {
                menuPosition.y = Screen.height * topMenu.dropScreenHeight;
                topMenu.CoordListState = LogicStates.Waiting;
                return;
            }

            // if (menuPosition.y > Screen.height * Swap)
            // {
            //     topMenu.State = LogicStates.Hiding;
            //     topMenu.ConstantTopPanel.SetActive(true);
            //     return;
            // }

            topMenu.CoordSelectMenu.transform.position = menuPosition;
        }

        private IEnumerator AutoPull()
        {
            var menuPosition = topMenu.CoordSelectMenu.transform.position;
            var midHeight =  Screen.height * topMenu.dropScreenHeight + 
                             (Screen.height - Screen.height * topMenu.dropScreenHeight) / 2;
            while (menuPosition.y <= dockPosition.y || menuPosition.y >= Screen.height * topMenu.dropScreenHeight)
            {
                var translation = menuPosition.y > midHeight ? Vector3.up : Vector3.down;
                
                if (menuPosition.y >= Screen.height * Swap)
                {
                    topMenu.ConstantTopPanel.SetActive(true);
                    yield break;
                }

                if (menuPosition.y <= Screen.height * topMenu.dropScreenHeight)
                {
                    topMenu.CoordSelectMenu.transform.position =
                        new Vector2(menuPosition.x, Screen.height * topMenu.dropScreenHeight);
                    yield break;
                }
                
                topMenu.CoordSelectMenu.transform
                    .Translate(translation * (Time.deltaTime * topMenu.transformFactor));
                menuPosition = topMenu.CoordSelectMenu.transform.position;
                yield return null;
            }
        }
    }
}
