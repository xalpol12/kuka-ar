using System;
using UnityEngine;

public class WebViewEvents : MonoBehaviour
{
    public static WebViewEvents Events;

    private void Awake()
    {
        Events = this;
    }

    public event Action<int> OnClickOpenMoreOptions;

    public void OnClickGoBack(int id)
    {
        OnClickOpenMoreOptions?.Invoke(id);
    }
}
