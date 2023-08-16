using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableStylingService : MonoBehaviour
{
    public static SelectableStylingService Instance;
    
    internal Sprite DefaultSprite;
    internal Sprite SelectedSprite;
    internal Sprite DefaultAddIcon;
    internal Sprite PressedAddIcon;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DefaultSprite = Resources.Load<Sprite>("Gradients/GreyListBar");
        SelectedSprite = Resources.Load<Sprite>("Fields/Selected");
        DefaultAddIcon = Resources.Load<Sprite>("Icons/circle");
        PressedAddIcon = Resources.Load<Sprite>("Icons/circlePress");
    }

    public void MarkAsUnselected(List<GameObject> allGridItems)
    {
        foreach (var item in allGridItems)
        {
            item.transform.GetComponent<Image>().sprite = DefaultSprite;
        }
    }
}
