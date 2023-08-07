using UnityEngine;

public class JogsController : MonoBehaviour
{
    public int id;
    public float transformFactor;
    public GameObject jogButton;
    public GameObject jogsValues;
    internal bool ShowJogs;
    private int defaultTransformFactor;
    void Start()
    {
        ShowJogs = false;
        defaultTransformFactor = 10;
        ValueCheck();
        
        MenuEvents.Event.OnClickJog += OnClickJog;
    }

    private void OnClickJog(int gui)
    {
        if (id == gui)
        {
            ShowJogs = !ShowJogs;
        }
    }

    private void ValueCheck()
    {
        if (transformFactor is > 200f or < 0f)
        {
            transformFactor = defaultTransformFactor;
        }
    }
}