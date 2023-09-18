using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.EventSystem.Services.Menu
{
    public class SelectableStylingService : MonoBehaviour
    {
        public static SelectableStylingService Instance;
    
        internal Sprite DefaultSprite;
        internal Sprite SelectedSprite;
        internal Sprite DefaultSticker;
        internal Sprite DefaultAddIcon;
        internal Sprite PressedAddIcon;
        internal Sprite InvalidSelectable;
        internal Sprite DefaultInputField;
        internal Sprite DefaultNoFrame;

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
