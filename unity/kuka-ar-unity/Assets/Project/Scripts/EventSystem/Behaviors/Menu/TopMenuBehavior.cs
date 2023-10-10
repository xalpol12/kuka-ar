using System.Collections;
using Project.Scripts.EventSystem.Controllers.Menu;
using Project.Scripts.EventSystem.Enums;
using UnityEngine;

namespace Project.Scripts.EventSystem.Behaviors.Menu
{
    public class TopMenuBehavior : MonoBehaviour
    {
        private TopMenuController topMenuController;
        private GameObject coordSelectMenu;
        private GameObject constantTopPanel;
        private Vector3 dockPosition;
        
        private const float Swap = 0.95f;

        private void Start()
        {
            topMenuController = GetComponent<TopMenuController>();
            coordSelectMenu = topMenuController.TopMenu.GetComponent<RectTransform>()
                .Find("Selectable").GetComponent<RectTransform>().gameObject;
            constantTopPanel = topMenuController.TopMenu.GetComponent<RectTransform>()
                .Find("ConstantInfoPanel").GetComponent<RectTransform>().gameObject;
            
            dockPosition = coordSelectMenu.transform.position;
        }

        private void Update()
        {
            if (topMenuController.CoordListState == LogicStates.Running)
            {
                DragSlider();
            } else if (topMenuController.CoordListState == LogicStates.AutoPulling)
            {
                StartCoroutine(AutoPull());
                topMenuController.CoordListState = LogicStates.Waiting;
            }

            if (topMenuController.ConstantPanelState != LogicStates.Running) return;
            
            StartCoroutine(DropTopMenu());
            topMenuController.ConstantPanelState = LogicStates.Waiting;
        }

        private IEnumerator DropTopMenu()
        {
            constantTopPanel.SetActive(false);
            topMenuController.jogs.SetActive(false);
            
            var translation = Vector3.down * (Time.deltaTime * topMenuController.transformFactor);
            var menuPosition = coordSelectMenu.transform.position + translation;

            while (menuPosition.y > Screen.height * topMenuController.dropScreenHeight)
            {
                coordSelectMenu.transform.Translate(translation);
                menuPosition = coordSelectMenu.transform.position + translation;
                yield return null;
            }

            coordSelectMenu.transform.position = menuPosition;
        }

        private void DragSlider()
        {
            var menuPosition = new Vector3(dockPosition.x ,Input.mousePosition.y);
            if (menuPosition.y < Screen.height * topMenuController.dropScreenHeight)
            {
                menuPosition.y = Screen.height * topMenuController.dropScreenHeight;
                topMenuController.CoordListState = LogicStates.Waiting;
                return;
            }

            coordSelectMenu.transform.position = menuPosition;
        }

        private IEnumerator AutoPull()
        {
            var menuPosition = coordSelectMenu.transform.position;
            var midPoint =  Screen.height * topMenuController.dropScreenHeight + 
                             (Screen.height - Screen.height * topMenuController.dropScreenHeight) / 2;
            while (menuPosition.y <= dockPosition.y || 
                   menuPosition.y >= Screen.height * topMenuController.dropScreenHeight)
            {
                var translation = menuPosition.y > midPoint ? Vector3.up : Vector3.down;
                
                if (menuPosition.y >= Screen.height * Swap)
                {
                    constantTopPanel.SetActive(true);
                    topMenuController.jogs.SetActive(true);
                    yield break;
                }

                if (menuPosition.y <= Screen.height * topMenuController.dropScreenHeight)
                {
                    coordSelectMenu.transform.position = 
                        new Vector2(menuPosition.x, Screen.height * topMenuController.dropScreenHeight);
                    yield break;
                }
                
                coordSelectMenu.transform
                    .Translate(translation * (Time.deltaTime * topMenuController.transformFactor));
                menuPosition = coordSelectMenu.transform.position;
                yield return null;
            }
        }
    }
}
