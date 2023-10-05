using System;
using UnityEngine;

namespace Project.Scripts.EventSystem.Controllers.Menu
{
    public class TopMenuController : MonoBehaviour
    {
        public int id;
        [NonSerialized] public GameObject CoordSelectMenu;
        [NonSerialized] public GameObject ConstantTopPanel;
        private void Start()
        {
            CoordSelectMenu = GetComponent<RectTransform>()
                .Find("Selectable").GetComponent<RectTransform>().gameObject;
            ConstantTopPanel = GetComponent<RectTransform>()
                .Find("ConstantInfoPanel").GetComponent<RectTransform>().gameObject;
        }

        // Update is called once per frame
        private void Update()
        {
        
        }
    }
}
