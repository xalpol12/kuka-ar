using UnityEngine;

public class JogsController : MonoBehaviour
{
    public int id;
    public float transformFactor;
    public GameObject jogs;
    private int defaultTransformFactor;
    internal bool ShowJogs;

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