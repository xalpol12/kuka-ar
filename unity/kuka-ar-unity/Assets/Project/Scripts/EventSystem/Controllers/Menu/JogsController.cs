using Project.Scripts.EventSystem.Enums;
using UnityEngine;

public class JogsController : MonoBehaviour
{
    public int id;
    public float transformFactor;
    public GameObject jogs;
    private bool showJogs;
    private int defaultTransformFactor;
    internal JogsControlService Service;
    internal LogicStates JogsTrigger;

    void Start()
    {
        Service = JogsControlService.Instance;
        
        
        showJogs = false;
        JogsTrigger = LogicStates.Waiting;
        defaultTransformFactor = 10;
        
        ValueCheck();
        
        MenuEvents.Event.OnClickJog += OnClickJog;
    }

    private void OnClickJog(int gui)
    {
        if (id == gui)
        {
            showJogs = !showJogs;
            JogsTrigger = showJogs ? LogicStates.Running : LogicStates.Hiding;
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