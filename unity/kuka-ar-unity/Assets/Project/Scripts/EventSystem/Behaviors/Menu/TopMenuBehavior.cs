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
            if (topMenu.State == LogicStates.Running)
            {
                StartCoroutine(DropTopMenu());
                topMenu.State = LogicStates.Waiting;
            } /*else if (topMenu.State == LogicStates.Hiding)
            {

            }*/
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
    }
}
