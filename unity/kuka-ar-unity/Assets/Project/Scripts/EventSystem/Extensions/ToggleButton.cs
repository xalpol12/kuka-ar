using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private RectTransform uiHandle;
    [SerializeField] private Color backgroundColor;
    [SerializeField] private Color handleColor;
    
    private Toggle toggle;
    private Vector2 handlePosition;
    private Color backgroundColorDefault, handleColorDefault;
    private Image backgroundImage, handleImage;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();

        handlePosition = uiHandle.anchoredPosition;
        backgroundImage = uiHandle.parent.GetComponent<Image>();
        handleImage = uiHandle.GetComponent<Image>();

        backgroundColorDefault = backgroundImage.color;
        handleColorDefault = handleImage.color;
        
        toggle.onValueChanged.AddListener(OnSwitch);

        if (toggle.isOn)
        {
            OnSwitch(true);
        }
    }

    private void OnSwitch(bool on)
    {
        uiHandle.anchoredPosition = on ? handlePosition * -1 : handlePosition;
        backgroundImage.color = on ? backgroundColor : backgroundColorDefault;
        handleImage.color = on ? handleColor : handleColorDefault;
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
