using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableStylingService : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite selectedSprite;

    private void Start()
    {
        defaultSprite = Resources.Load<Sprite>("Gradients/GreyListBar");
        selectedSprite = Resources.Load<Sprite>("Fields/Selected");
    }

    public void MarkAsUnselected(List<GameObject> allGridItems)
    {
        foreach (var item in allGridItems)
        {
            item.transform.GetComponent<Image>().sprite = defaultSprite;
        }
    }
}
