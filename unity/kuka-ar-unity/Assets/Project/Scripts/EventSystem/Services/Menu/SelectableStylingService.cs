using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Connectivity.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.EventSystem.Services.Menu
{
    public class SelectableStylingService : MonoBehaviour
    {
        public static SelectableStylingService Instance;
    
        internal Sprite DefaultSprite;
        [NonSerialized] public Sprite SelectedSprite;
        [NonSerialized] public Sprite DefaultSticker;
        [NonSerialized] public Sprite DefaultAddIcon;
        [NonSerialized] public Sprite PressedAddIcon;
        [NonSerialized] public Sprite InvalidSelectable;
        [NonSerialized] public Sprite DefaultInputField;
        [NonSerialized] public Sprite DefaultNoFrame;

        private Sprite defaultSelectableSprite;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            DefaultSprite = Resources.Load<Sprite>("Icons/cloudIcon");
            SelectedSprite = Resources.Load<Sprite>("Fields/Selected");
            DefaultSticker = Resources.Load<Sprite>("Logos/robotIcon");
            DefaultAddIcon = Resources.Load<Sprite>("Icons/circle");
            PressedAddIcon = Resources.Load<Sprite>("Icons/circlePress");
            InvalidSelectable = Resources.Load<Sprite>("Fields/SelectFieldInvalid");
            DefaultInputField = Resources.Load<Sprite>("Fields/InputField");

            defaultSelectableSprite = Resources.Load<Sprite>("Fields/InputField");
            DefaultNoFrame = Resources.Load<Sprite>("Gradients/GreyListBar");
        }

        public void MarkAsUnselected(List<GameObject> allGridItems, bool noFrame = false)
        {
            foreach (var item in allGridItems)
            {
                item.transform.GetComponent<Image>().sprite = noFrame ? DefaultNoFrame : defaultSelectableSprite;
            }
        }

        public void MarkAsUnselectedWithCondition(IEnumerable<GameObject> items, object condition)
        {
            if (condition is ButtonType)
            {
                foreach (var item in items.Where(item => item.name.Contains(condition.ToString())))
                {
                    item.transform.GetComponent<Image>().sprite = defaultSelectableSprite;
                }
            }
            else
            {
                throw new NotSupportedException();
            }
            
        }

        public IEnumerator FadeOutText(TMP_Text text)
        {
            yield return new WaitForSeconds(2);
            var i = 1f;
            while (i > -0.01f)
            {
                text.color = new Color(1, 0, 0, i);
                i -= 0.01f;
                yield return null;
            }
        }
    }
}
